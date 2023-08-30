using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ASNA.QSys.Expo.Model;

// Migrated on 8/25/2023 at 5:23 PM by ASNA Monarch(R) version 11.4.12.0
// Legacy location: library ERCAP, file QDDSSRC, member CUSTDSPF


namespace ACME.SunFarm.SunFarmViews
{
    [
        BindProperties,
        DisplayPage(FunctionKeys = "F3 03"),
        ExportSource(CCSID = 37)
    ]
    public class CUSTDSPF : DisplayPageModel
    {
        public SFLC_Model SFLC { get; set; }
        public CUSTREC_Model CUSTREC { get; set; }
        public SALESREC_Model SALESREC { get; set; }
        public KEYS_Model KEYS { get; set; }
        public MSGSFC_Model MSGSFC { get; set; }

        public CUSTDSPF()
        {
            SFLC = new SFLC_Model();
            CUSTREC = new CUSTREC_Model();
            SALESREC = new SALESREC_Model();
            KEYS = new KEYS_Model();
            MSGSFC = new MSGSFC_Model();
        }

        [
            SubfileControl(ClearRecords : "90",
                FunctionKeys = "F9 09;PageUp 51:!76;PageDown 50:!77",
                DisplayFields = "!90",
                DisplayRecords = "!90",
                Size = 14,
                IsExpandable = false,
                EraseFormats = "CUSTREC , SALESREC"
            )
        ]
        public class SFLC_Model : SubfileControlModel
        {
            public List<SFL1_Model> SFL1 { get; set; } = new List<SFL1_Model>();

            [Char(10, Upper = false, OutputData=false)]
            public string SETNAME { get; set; }

            public class SFL1_Model : SubfileRecordModel
            {
                [Char(1, Protect = "*True")]
                public string SFCOLOR { get; set; }

                [Values(typeof(Decimal),"00","02","03","05","07","09","10","11")]
                [Dec(2, 0)]
                public decimal SFSEL { get; set; }

                [Dec(6, 0)]
                public decimal SFCUSTNO { get; private set; } // CUSTOMER NUMBER

                [Char(40)]
                public string SFNAME1 { get; private set; }

                [Char(25)]
                public string SFCSZ { get; private set; } // CITY-STATE-ZIP

            }

        }

        [
            Record(FunctionKeys = "F4 04;F6 06:!30;F11 11:!30;F12 12",
                EraseFormats = "KEYS , SALESREC , SFLC"
            )
        ]
        public class CUSTREC_Model : RecordModel
        {
            [Char(10)]
            private string CSRREC
            {
                get => CursorLocationFormatName;
                set { }
            }

            [Char(10)]
            private string CSRFLD
            {
                get => CursorLocationFieldName;
                set { }
            }

            [Byte(Alias = "@SFNAME")]
            public byte aSFNAME { get; private set; } // Color Attr: Name

            [Byte(Alias = "@CONTACT")]
            public byte aCONTACT { get; private set; } // Color Attr: Contact

            [Byte(Alias = "@FAXPHONE")]
            public byte aFAXPHONE { get; private set; } // Color Attr: Phone&Fax

            [Char(10)]
            public string SCPGM { get; private set; }

            [Dec(6, 0)]
            public decimal SFCUSTNO { get; private set; } // CUSTOMER NUMBER

            [Char(40)]
            public string SFOLDNAME { get; private set; }

            [Char(40, Alias = "#SFNAME",Upper = false,ProtectCodeFieldName = nameof(aSFNAME))]
            public string hSFNAME { get; set; }

            [Char(35, Upper = false)]
            public string SFADDR1 { get; set; }

            [Char(35, Upper = false)]
            public string SFADDR2 { get; set; }

            [Char(30, Upper = false)]
            public string SFCITY { get; set; }

            [Char(2)]
            public string SFSTATE { get; set; }

            [Char(10)]
            public string SFPOSTCODE { get; set; }

            [Dec(10, 0, ProtectCodeFieldName = nameof(aFAXPHONE))]
            public decimal SFFAX { get; set; }

            [Char(20, ProtectCodeFieldName = nameof(aFAXPHONE))]
            public string SFPHONE { get; set; }

            [Char(1, Alias = "#$STATUS")]
            public string hsSTATUS { get; set; }

            [Char(40, ProtectCodeFieldName = nameof(aCONTACT))]
            public string SFCONTACT { get; set; }

            [Char(40, ProtectCodeFieldName = nameof(aCONTACT))]
            public string SFCONEMAL { get; set; }

            [Char(1)]
            public string SFYN01 { get; set; }

        }

        [
            Record(FunctionKeys = "F12 12",
                EraseFormats = "CUSTREC , KEYS , SFLC"
            )
        ]
        public class SALESREC_Model : RecordModel
        {
            [Char(10)]
            public string SCPGM { get; private set; }

            [Dec(6, 0)]
            public decimal SFCUSTNO { get; private set; } // CUSTOMER NUMBER

            [Char(40)]
            public string SFNAME { get; private set; }

            [Dec(13, 2)]
            public decimal SFSALES { get; private set; }

            [Dec(13, 2)]
            public decimal SFRETURNS { get; private set; }

        }

        [
            Record(EraseFormats = "CUSTREC , SALESREC")
        ]
        public class KEYS_Model : RecordModel
        {
        }

        [
            SubfileControl(ClearRecords : "",
                ProgramQ = "@PGMQ",
                DisplayFields = "*True",
                DisplayRecords = "*True",
                InitializeRecords = "*True",
                Size = 10
            )
        ]
        public class MSGSFC_Model : SubfileControlModel
        {
            public List<MSGSF_Model> MSGSF { get; set; } = new List<MSGSF_Model>();

            [Char(10, Alias = "@PGMQ")]
            public string aPGMQ { get; private set; }

            [
                SubfileRecord(IsMessageSubfile = true)
            ]
            public class MSGSF_Model : SubfileRecordModel
            {
                [Char(4, Alias = "@MSGKY")]
                public string aMSGKY { get; private set; }

                [Char(10, Alias = "@PGMQ")]
                public string aPGMQ { get; private set; }

                [Char(128)]
                public string MessageText { get; private set; }

            }

        }

    }
}
