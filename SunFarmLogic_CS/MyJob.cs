// Translated from Encore to C# on 10/5/2023 at 10:57:08 AM by ASNA Encore Translator® version 4.0.18.0
using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using System;
using ACME.SunFarm;
using ASNA.QSys.Runtime.JobSupport;
namespace ACME.SunFarmCustomers_Job
{


    [ProgramIndicators(INLRName = "_INLR", INName = "_IN", INRTName = "_INRT")]
    public partial class MyJob : InteractiveJob
    {
        protected Indicator _INLR;
        protected Indicator _INRT;
        protected IndicatorArray<Len<_1, _0, _0>> _IN;
        protected dynamic _DynamicCaller;
        public Database MyDatabase = new Database("SQL_LINEAR");
        public Database MyPrinterDB = new Database("SQL_LINEAR");


        override protected Database getDatabase()
        {
            return MyDatabase;
        }

        override protected Database getPrinterDB()
        {
            return MyPrinterDB;
        }

        override public void Dispose(bool disposing)
        {
            if (disposing)
            {

                MyDatabase.Close();
                MyPrinterDB.Close();

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
            MyPrinterDB.Open();

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
