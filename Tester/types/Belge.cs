using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkysoft.gib.tester.types
{
    public class Belge: Zarf
    {
        public Belge() { }
        public Belge(Zarf envelope)
        {
            this.GonderenVKN_TCKN = envelope.GonderenVKN_TCKN;
            this.GonderenUnvan = envelope.GonderenUnvan;
            this.GB = envelope.GB;
            this.AliciVKN_TCKN = envelope.AliciVKN_TCKN;
            this.AliciUnvan = envelope.AliciUnvan;
            this.PK = envelope.PK;
            this.ZarfEttn = envelope.ZarfEttn;
            this.ZarfTarih = envelope.ZarfTarih;
            this.EnvType = envelope.EnvType;
            this.ElmntType = envelope.ElmntType;
        }

        public string BelgeEttn { get; set; }
        public string BelgeId { get; set; }
        public string BelgeTarihi { get; set; }
        public Enums.InvType BelgeTipi { get; set; }
        public Enums.InvProfile BelgeProfili { get; set; }
        public string Referans { get; internal set; }
        public string ReferansKodu { get; internal set; }
        public string ReferansMetin { get; internal set; }
    }
}
