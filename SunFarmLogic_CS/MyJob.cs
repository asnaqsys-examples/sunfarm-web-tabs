// Translated from Encore to C# on 9/7/2023 at 9:29:34 AM by ASNA Encore Translator® version 4.0.17.0
using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using System;
using ACME.SunFarm;
using ASNA.QSys.Runtime.JobSupport;
namespace ACME.SunFarmCustomers_Job
{


    public partial class MyJob : InteractiveJob
    {
        protected Indicator _INLR;
        protected Indicator _INRT;
        protected IndicatorArray<Len<_1, _0, _0>> _IN;
        protected dynamic _DynamicCaller;
        public Database MyDatabase = new Database("SQL_LINEAR");
        // DclDB Name(MyPrinterDB) DBName("Local") Access(*Public)


        override protected Database getDatabase()
        {
            return MyDatabase;
        }

        //   BegFunc getPrinterDB Type(Database) Access(*Protected) Modifier(*Overrides)
        //   LeaveSR MyPrinterDB
        //   EndFunc


        override public void Dispose(bool disposing)
        {
            if (disposing)
            {

                MyDatabase.Close();
                // Disconnect  MyPrinterDB

            }
            base.Dispose(disposing);
        }

        static MyJob()
        {
            Database.PrepareNameStore<MyJob>(NameStoreOptions.UseJsonDefaultPath);
        }

        public static MyJob JobFactory()
        {
            MyJob job = null;

            job = new MyJob();
            return job;
        }

        override protected void ExecuteStartupProgram()
        {
            Indicator _LR = '0';
            MyDatabase.Open();

            // Connect     MyPrinterDB


            _DynamicCaller.CallD("ACME.SunFarm.RUNCI", out _LR);
        }



        public MyJob()
        {
            _IN = new IndicatorArray<Len<_1, _0, _0>>((char[])null);
            _instanceInit();
        }

        void _instanceInit()
        {
            _DynamicCaller = new DynamicCaller(this);
        }
    }

}
