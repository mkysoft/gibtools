using com.mkysoft.helper;
using com.mkysoft.gib.tester.helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.mkysoft.gib.tester
{
    public partial class frmMain : Form
    {
        private DataTable dtMusteriler = new DataTable();

        public frmMain()
        {
            InitializeComponent();
        }

        private async void BtnTekFatura_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            try
            {
                FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);
                string sonID = await helper.Test.Fatura(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, 1, 1, belge, envelope);
                txtFaturaID.Text = helper.Fatura.SonrakiFaturaID(sonID);
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;
        }

        private void FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf zarf)
        {
            zarf = new types.Zarf() { GonderenVKN_TCKN = txtGondericiVKN.Text };
            zarf.GonderenUnvan = txtGondericiUnvan.Text;
            zarf.AliciUnvan = txtAliciUnvan.Text;
            zarf.AliciVKN_TCKN = txtAliciVKN.Text;
            zarf.GB = txtGondericiGB.Text;
            zarf.PK = txtAliciPK.Text;

            belge = new types.Belge(zarf);
            belge.BelgeProfili = types.Enums.InvProfile.TEMELFATURA;
            belge.BelgeTipi = types.Enums.InvType.SATIS;
            belge.BelgeId = txtFaturaID.Text;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            wcf.Helper.Start(lbLog, pbIs);
            dgZarflar.DataSource = bsZarflar.DataSource = wcf.Helper.dtZarflar;
            dgBelgeler.DataSource = bsBelgeler.DataSource = wcf.Helper.dtBelgeler;
            cmbDevice.Items.AddRange(Enum.GetNames(typeof(xades.Enums.Device)));
            LoadConfig();
        }

        private void LoadConfig()
        {
            var ini = new IniFile();
            LoadConfig(ini, this);
            //Müşteriler
            dtMusteriler.TableName = "Musteriler";
            dtMusteriler.Columns.Add("TEST", typeof(string));
            dtMusteriler.Columns.Add("VKN", typeof(string));
            dtMusteriler.Columns.Add("UNVAN", typeof(string));
            dtMusteriler.Columns.Add("GB", typeof(string));
            dtMusteriler.Columns.Add("PK", typeof(string));
            dtMusteriler.Columns.Add("ID", typeof(string));
            dtMusteriler.Columns.Add("TOKENDEVICE", typeof(xades.Enums.Device));
            dtMusteriler.Columns.Add("TOKENNAME", typeof(string));
            dtMusteriler.Columns.Add("TOKENSERIAL", typeof(string));
            dtMusteriler.Columns.Add("TOKENPIN", typeof(string));
            dgMusteriler.DataSource = dtMusteriler;

            if (File.Exists("Musteriler.xml"))
                dtMusteriler.ReadXml("Musteriler.xml");
            if (dtMusteriler.Rows.Count == 0)
            {
                dtMusteriler.Rows.Add(dtMusteriler.NewRow());
                dtMusteriler.Rows.Add(dtMusteriler.NewRow());
                dtMusteriler.Rows.Add(dtMusteriler.NewRow());
            }
        }

        private void LoadConfig(IniFile ini, Control control)
        {
            var controls = control.Controls.Cast<Control>();
            foreach (var item in controls)
            {
                if (item.HasChildren)
                    LoadConfig(ini, item);
                if (item.Tag == null || item.Tag.ToString() != "Config")
                    continue;
                if (ini.KeyExists(item.Name))
                    item.Text = ini.Read(item.Name);
            }
        }

        private void FrmMain_Leave(object sender, EventArgs e)
        {
            wcf.Helper.Stop();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtMusteriler.WriteXml("Musteriler.xml");
            var ini = new IniFile();
            SaveConfig(ini, this);
        }

        private void SaveConfig(IniFile ini, Control control)
        {
            var controls = control.Controls.Cast<Control>();
            foreach (var item in controls)
            {
                if (item.HasChildren)
                    SaveConfig(ini, item);
                if (item.Tag == null || item.Tag.ToString() != "Config")
                    continue;
                ini.Write(item.Name, item.Text);
            }
        }

        private async void tmrJob_Tick(object sender, EventArgs e)
        {
            tmrJob.Enabled = false;
            if (wcf.Helper.dtZarflar.Rows.Count > 0)
            {
                try 
                {
                    await ZarflariIsle();
                } 
                catch (Exception ex) {
                    wcf.Helper.Log("ZarflariIsle: " + ex.ToString());
                }
            }
            tmrJob.Enabled = true;
        }

        private Task ZarflariIsle()
        {
            return Task.Run(() =>
            {
                var query = "ZTIPI IN (" + (int)types.Enums.EnvType.SENDERENVELOPE + "," + (int)types.Enums.EnvType.POSTBOXENVELOPE + "," + (int)types.Enums.EnvType.USERENVELOPE + ")";
                query   += " AND DURUM NOT IN (" + (int)types.Enums.Durum.TAMAM + "," + +(int)types.Enums.Durum.HATA + ")";
                var result = wcf.Helper.dtZarflar.Select(query);
                for (int i = 0; i < result.Length; i++)
                {
                    var zarf = result[i];

                    var envelope = Zarf.RowToClass(zarf);
                    var yon = (types.Enums.Yon)(zarf["YON"]);
                    var durum = (types.Enums.Durum)(zarf["DURUM"]);

                    if (durum == types.Enums.Durum.YENI)
                    {
                        switch(yon)
                        {
                            case types.Enums.Yon.GELEN:
                                helper.Zarf.GelenZarfiCevapla(envelope, txtGIBServisAdresi.Text);
                                break;
                            case types.Enums.Yon.GIDEN:
                                helper.Zarf.ZarfiSorgula(envelope, txtGIBServisAdresi.Text);
                                break;
                        }
                    }

                    if (durum == types.Enums.Durum.CEVAP)
                    {
                        switch (yon)
                        {
                            case types.Enums.Yon.GELEN:
                                helper.Zarf.ZarfiSorgula(envelope, txtGIBServisAdresi.Text);
                                break;
                            case types.Enums.Yon.GIDEN:
                                helper.Zarf.ZarfiSorgula(envelope, txtGIBServisAdresi.Text);
                                break;
                        }
                    }

                }
            });
        }

        private async void btn10Fatura_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            try
            {
                FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);
                string sonID = await helper.Test.Fatura(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, 10, 1, belge, envelope);
                txtFaturaID.Text = helper.Fatura.SonrakiFaturaID(sonID);
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;

        }

        private void lbLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.C))
            {
                Clipboard.SetText(this.lbLog.SelectedItem.ToString());
            }
        }

        private void btnSorgula_Click(object sender, EventArgs e)
        {
            var appres = Gib.Request(txtGIBServisAdresi.Text, txtZarf.Text);
            txtSonuc.Text = appres;
            txtSonuc.SelectionStart = txtSonuc.Text.Length;
            txtSonuc.ScrollToCaret();
        }

        private async void btn100Fatura10Zarf_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            try
            {
                FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);
                string sonID = await helper.Test.Fatura(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, 100, 10, belge, envelope);
                txtFaturaID.Text = helper.Fatura.SonrakiFaturaID(sonID);
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;

        }

        private async void btnHataliZarf_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            try
            {
                await helper.Test.HataliZarf(txtGIBServisAdresi.Text);
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;
        }

        private async void btn100Fatura_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            try
            {
                FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);
                string sonID = await helper.Test.Fatura(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, 100, 1, belge, envelope);
                txtFaturaID.Text = helper.Fatura.SonrakiFaturaID(sonID);
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;
        }

        private async void btn1Fatura10Zarf_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            try
            {
                FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);
                string sonID = await helper.Test.Fatura(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, 1, 10, belge, envelope);
                txtFaturaID.Text = helper.Fatura.SonrakiFaturaID(sonID);
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;
        }

        private async void btnTicariFatura_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);
            belge.BelgeProfili = types.Enums.InvProfile.TICARIFATURA;

            try
            {
                string sonID = await helper.Test.Fatura(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, 1, 1, belge, envelope);
                rtbPosta.AppendText("\r\n" + envelope.ZarfEttn + " numaralı zarf içindeki " + belge.BelgeId + " numaralı ticari fatura ile KABUL testi yapılmıştır. (6.1.1)");
                txtFaturaID.Text = helper.Fatura.SonrakiFaturaID(sonID);
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;
        }

        private async void btnTicariFaturaRed_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            try
            {
                FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);
                belge.BelgeProfili = types.Enums.InvProfile.TICARIFATURA;
                string sonID = await helper.Test.Fatura(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, 1, 1, belge, envelope);
                rtbPosta.AppendText("\r\n" + envelope.ZarfEttn + " numaralı zarf içindeki " + belge.BelgeId + " numaralı ticari fatura ile RED testi yapılmıştır. (6.1.1)");
                txtFaturaID.Text = helper.Fatura.SonrakiFaturaID(sonID);
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;
        }

        private void btnCevapKabul_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;


            CommercialResponse(types.Enums.Cevap.KABUL);

            (sender as Button).Enabled = true;
            (sender as Button).BackColor = Color.Green;
        }

        private void btnCevapRed_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            CommercialResponse(types.Enums.Cevap.RED);

            (sender as Button).Enabled = true;
            (sender as Button).BackColor = Color.Green;
        }

        private async void btnIstisnaFatura_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);

            try
            {
                string sonID = await helper.Test.Istisna(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, belge, envelope);
                rtbPosta.AppendText("\r\n" + envelope.ZarfEttn + " numaralı zarf içindeki " + belge.BelgeId + " numaralı fatura ile İstisna testi yapılmıştır. (7.a)");
                txtFaturaID.Text = helper.Fatura.SonrakiFaturaID(sonID);
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;
        }

        private async void CommercialResponse(types.Enums.Cevap cevap)
        {
            try
            {
                await Fatura.Cevapla(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, cevap);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dgBelgeler_FilterStringChanged(object sender, EventArgs e)
        {
            bsBelgeler.Filter = dgBelgeler.FilterString;
        }

        private void dgBelgeler_SortStringChanged(object sender, EventArgs e)
        {
            bsBelgeler.Sort = dgBelgeler.SortString;
        }

        private void dgZarflar_FilterStringChanged(object sender, EventArgs e)
        {
            bsZarflar.Filter = dgZarflar.FilterString;
        }

        private void dgZarflar_SortStringChanged(object sender, EventArgs e)
        {
            bsZarflar.Sort = dgZarflar.SortString;
        }

        private async void btnKullaniciAc_Click(object sender, EventArgs e)
        {
            await Kullanici(false, (sender as Button), types.Enums.MusteriTest.M1E1);
        }

        private async void btnKullaniciSil_Click(object sender, EventArgs e)
        {
            await Kullanici(true, (sender as Button), types.Enums.MusteriTest.M1E1);
        }

        private Task Kullanici(bool Iptal, Button button, types.Enums.MusteriTest test)
        {
            return Task.Run(() =>
            {
                button.Enabled = false;

                FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);
                envelope.EnvType = types.Enums.EnvType.USERENVELOPE;
                if (Iptal)
                    envelope.ElmntType = types.Enums.ElmntType.CANCELUSERACCOUNT;
                else
                    envelope.ElmntType = types.Enums.ElmntType.PROCESSUSERACCOUNT;
                envelope.GB = "usergb";
                envelope.AliciVKN_TCKN = "3900383669";
                envelope.AliciUnvan = "Gelir İdaresi Başlanlığı";
                envelope.PK = "GIB";
                var musteriler = new List<types.Musteri>();
                var result = dtMusteriler.Select("TEST = '" + test.ToString() + "'");
                string VKN = string.Empty;
                types.Musteri musteri = null;
                for (int i = 0; i < result.Length; i++)
                {
                    var yeni = OkuMusteri(result[i]);
                    if (musteri == null)
                        musteri = yeni;
                    else if (musteri.VKN.Equals(yeni.VKN))
                        musteri.Etiketler.AddRange(yeni.Etiketler);
                    else
                    {
                        musteriler.Add(musteri);
                        musteri = yeni;
                    }
                }
                if (!musteriler.Exists(x=>x.VKN.Equals(musteri.VKN)))
                    musteriler.Add(musteri);
                try
                {
                    helper.Test.Kullanici(Iptal, txtGIBServisAdresi.Text, cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, musteriler, envelope);
                    button.BackColor = Color.Green;
                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                button.Enabled = true;
            });
        }

        private types.Musteri OkuMusteri(DataRow row)
        {
            var musteri = new types.Musteri();
            musteri.TokenDevice = (xades.Enums.Device)row["TOKENDEVICE"];
            musteri.TokenName = row["TOKENNAME"].ToString();
            musteri.TokenSerial = row["TOKENSERIAL"].ToString();
            musteri.TokenPin = row["TOKENPIN"].ToString();
            musteri.VKN = row["VKN"].ToString();
            musteri.Unvan = row["UNVAN"].ToString();
            musteri.ID = row["ID"].ToString();
            musteri.Etiketler = new List<types.Etiket>();
            musteri.Etiketler.Add(new types.Etiket() { 
                GB = row["GB"].ToString(), 
                PK = row["Pk"].ToString() 
            });
            return musteri;
        }

        private async void btnKullaniciAcCokluEtiket_Click(object sender, EventArgs e)
        {
            await Kullanici(false, (sender as Button), types.Enums.MusteriTest.M1E2);
        }

        private async void btnKullaniciIptalCokluEtiket_Click(object sender, EventArgs e)
        {
            await Kullanici(true, (sender as Button), types.Enums.MusteriTest.M1E2);
        }

        private async void btnCokluKullaniciAc_Click(object sender, EventArgs e)
        {
            await Kullanici(false, (sender as Button), types.Enums.MusteriTest.M3E1);
        }

        private async void btnCokluKullaniciIptal_Click(object sender, EventArgs e)
        {
            await Kullanici(true, (sender as Button), types.Enums.MusteriTest.M3E1);
        }

        private async void btnCokluKullaniciAcCokluEtiket_Click(object sender, EventArgs e)
        {
            await Kullanici(false, (sender as Button), types.Enums.MusteriTest.M3E2);
        }

        private async void btnCokluKullaniciIptalCokluEtiket_Click(object sender, EventArgs e)
        {
            await Kullanici(true, (sender as Button), types.Enums.MusteriTest.M3E2);
        }

        private async void btnPostaKutusu_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;

            FaturaVeZarfDoldur(out types.Belge belge, out types.Zarf envelope);

            var musteriler = new List<types.Musteri>();
            var result = dtMusteriler.Select("TEST = '" + types.Enums.MusteriTest.M3E1.ToString() + "'");
            foreach(var row in result)
                musteriler.Add(OkuMusteri(row));
            try
            {
                if (musteriler.Count < 3)
                    throw new ApplicationException("En az 3 müşteri olması gerekir");
                int zarfSay = 10 / musteriler.Count;
                foreach(var musteri in musteriler)
                {
                    musteri.FaturaSay = 100;
                    musteri.ZarfSay = zarfSay;
                }
                musteriler.Last<types.Musteri>().ZarfSay += 10 - zarfSay * musteriler.Count;


                await helper.Test.PostaKutusu(cmbDevice.SelectedItem.ToString(), txtTokenName.Text, txtTokenSerial.Text, txtTokenPin.Text, txtGIBServisAdresi.Text, envelope, musteriler);

                foreach (var musteri in musteriler)
                {
                    var rows = dtMusteriler.Select("VKN = '" + musteri.VKN + "'");
                    foreach (var row in rows)
                        row["ID"] = musteri.ID;
                }
                (sender as Button).BackColor = Color.Green;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            (sender as Button).Enabled = true;
        }
    }
}
