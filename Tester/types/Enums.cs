using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkysoft.gib.tester.types
{
    public class Enums
    {
        public static readonly string NS_Invoice = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";
        public static readonly string NS_cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
        public static readonly string NS_cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
        public static readonly string NS_sh = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader";
        public static readonly string NS_ef = "http://www.efatura.gov.tr/package-namespace";
        public static readonly string NS_ext = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2";
        public static readonly string NS_oa = "http://www.openapplications.org/oagis/9";
        public static readonly string NS_Kullanici = "http://www.hr-xml.org/3";

        public enum ElmntType
        {
            INVOICE,
            APPLICATIONRESPONSE,
            PROCESSUSERACCOUNT,
            CANCELUSERACCOUNT
        }

        public enum EnvType
        {
            SENDERENVELOPE,
            SYSTEMENVELOPE,
            POSTBOXENVELOPE,
            USERENVELOPE
        }

        public enum InvType
        {
            YOK,
            SATIS,
            ISTISNA
        }

        public enum InvProfile
        {
            YOK,
            TICARIFATURA,
            TEMELFATURA
        }

        public enum Yon
        {
            GELEN,
            GIDEN
        }
        public enum Durum
        {
            YENI,
            CEVAP,
            TAMAM,
            HATA
        }

        public enum Cevap
        {
            KABUL,
            RED
        }

        public enum MusteriTest
        {
            M1E1,
            M1E2,
            M3E1,
            M3E2
        }
    }
}
