// Translated from Encore to C# on 9/7/2023 at 9:29:34 AM by ASNA Encore Translator® version 4.0.17.0
// ASNA Monarch(R) version 11.4.12.0 at 8/25/2023
// Migrated source location: library ERCAP, file QRPGSRC, member CUSTINQ

using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using ACME.SunFarmCustomers_Job;

using System;
using ASNA.QSys.Runtime.JobSupport;


namespace ACME.SunFarm
{
    [CallerActivationGroup(), System.ComponentModel.Description("Customer Inquiry and Maintenance")]
    [ProgramEntry("_ENTRY")]
    public partial class CUSTINQ : Program
    {
        protected dynamic _DynamicCaller;

        //      ********************************************************************
        //       JB   8/27/2004   Added F4 (Prompt) options.
        //       JB   8/30/2004   Added 'submit job' option.
        //       JB   9/01/2004   Fixed record locking problem.
        //       JB   5/19/2005   Added colouring to the name/addr subfile.
        //                        Added cursor postioning to the detail format.
        //                        Added option to print sales online or in batch.
        //                        Changed the prompt display to a window.
        //       JB   5/23/2005   Added /COPY for reading CUSTOMERL1.
        //                        Added customer name to the sales summary format.
        //                        Added renaming of CUSTOMERL1 record format.
        //       JB   5/24/2005   Simplified the LoadSfl subroutine.
        //       JB   5/31/2005   Added PageDown/RollUp processing.
        //       JB   6/01/2005   Renamed CMMASTER* files to CUSTOMER*.
        //       JB  14/02/2006   Fixed bug in page up on the customer subfile -
        //                        a customer record was skipped every time.
        //      ********************************************************************
        //       INDICATORS:
        //         03     F3 pressed
        //         09     F9 pressed
        //         40-44  Cursor positioning
        //         50     PageUp pressed
        //         51     PageDown pressed
        //         66     EOF reading on the subfile
        //         76     BOF reading CUSTOMERL2
        //         77     EOF reading CUSTMOMER2
        //         88     LR seton in a called program
        //         99     General error indicator
        //      ********************************************************************
        WorkstationFile CUSTDSPF;
        //                                    HANDLER('ASNAWINGS')
        DatabaseFile CUSTOMERL2;
        DatabaseFile CUSTOMERL1;
        DatabaseFile CUSTOMERL1_ReadOnly;
        //********************************************************************
        DataStructure _DS0 = new (3);
        FixedDecimal<_3, _0> hpNbrs { get => _DS0.GetZoned(0, 3, 0); set => _DS0.SetZoned(value, 0, 3, 0); } 
        FixedString<_3> hpNbrsAlf { get => _DS0.GetString(0, 3); set => _DS0.SetString((string)value, 0, 3); } 

        FixedDecimalArray<_20, _9, _0> pNumbers;
        FixedStringArray<_20, _1> pTypes;
        FixedString<_7> MID;
        FixedString<_30> MTX;
        FixedDecimal<_13, _2> Sales;
        FixedDecimal<_13, _2> Returns;
        FixedString<_1> LockRec;
        FixedDecimal<_9, _0> CmCusth;
        FixedString<_13> SalesCh;
        FixedString<_13> ReturnsCh;
        FixedString<_9> CmCusthCH;
        FixedDecimal<_5, _0> X;
        DataStructure _DS1 = new (9);
        FixedDecimal<_9, _0> SVCUSTNO { get => _DS1.GetZoned(0, 9, 0); set => _DS1.SetZoned(value, 0, 9, 0); } 
        FixedString<_9> SVCUSTNOa { get => _DS1.GetString(0, 9); set => _DS1.SetString((string)value, 0, 9); } 

        // Customer DS
        DataStructure CUSTDS;

        //       Open Feedback Area
        //       Input/Output Feedback Information
        //                                                                         * 241-242 not used
        //       Display Specific Feedback Information
        //                                                                         *  cursor location
        int HRC02;
        FixedDecimal<_5, _0> SVS02;
        //********************************************************************

        // Fields defined in main C-Specs declared now as Global fields (Monarch generated)
        FixedString<_1> AddUpd;
        FixedString<_40> Name40;
        FixedDecimal<_9, _0> ORDCUST;
        FixedDecimal<_9, _0> pNumber;
        FixedString<_10> pResult;
        FixedDecimal<_4, _0> savrrn;
        FixedDecimal<_4, _0> sflrrn;
        FixedDecimal<_9, _0> TEMPNO;

        // PLIST(s) relocated by Monarch
        
        // KLIST(s) relocated by Monarch
        
#region Constructor and Dispose 
        public CUSTINQ()
        {
            _instanceInit();
            // Initialization of Data Structure fields (Monarch generated)
            Reset_hpNbrs();

            CUSTOMERL1.Open(CurrentJob.Database, AccessMode.RWCD, false, false, ServerCursors.Default);
            CUSTOMERL1_ReadOnly.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            CUSTOMERL2.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
        }

        override public void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                CUSTDSPF.Close();
                CUSTOMERL2.Close();
                CUSTOMERL1.Close();
                CUSTOMERL1_ReadOnly.Close();
            }
            base.Dispose(disposing);
        }


#endregion
        void StarEntry(int cparms)
        {
            Indicator _LR = '0';
            int _RrnTmp = 0;
            FixedString<_9> _SVCUSTNOaProxy = "";
            do
            {
                do
                {
                    // KLIST "KeyMastL2" moved by Monarch to global scope.
                    //********************************************************************
                    _IN[90] = '0';
                    CUSTDSPF.Write("MSGSFC", _IN.Array);
                    CUSTDSPF.Write("KEYS", _IN.Array);
                    CUSTDSPF.ExFmt("SFLC", _IN.Array);
                    ClearMsgs();
                    //--------------------------------------------------------------------
                    if ((bool)_IN[3])
                    {
                        _INLR = '1';
                        break;
                        // PageUp-RollDown
                    }
                    else if ((bool)_IN[50])
                    {
                        CUSTOMERL2.Seek(SeekMode.SetLL, CMNAME);
                        LoadSfl();
                        // Work with spooled files
                    }
                    else if ((bool)_IN[9])
                    {
                        _DynamicCaller.CallD("ACME.SunFarm.WWSPLF", out _LR);
                    }
                    else if (!SETNAME.IsBlanks())
                    {
                        Name40 = (string)SETNAME;
                        CUSTOMERL2.Seek(SeekMode.SetLL, Name40);
                        LoadSfl();
                        // PageDown-RollUp
                    }
                    else if ((bool)_IN[51])
                    {
                        ReadBack();
                        LoadSfl();
                    }
                    else if (!SETNAME.IsBlanks())
                    {
                        Name40 = (string)SETNAME;
                        CUSTOMERL2.Seek(SeekMode.SetLL, Name40);
                        LoadSfl();
                    }
                    else
                    {

                        // Process any individual Subfile record Option Selections

                        _IN[30] = '0';
                        _IN[66] = '0';
                        savrrn = sflrrn;
                        pNumbers.Initialize(0m);
                        hpNbrs = 0m;
                        do
                        {
                            _RrnTmp = (int)sflrrn;
                            _IN[66] = CUSTDSPF.ReadNextChanged("SFL1", ref _RrnTmp, _IN.Array) ? '0' : '1';
                            sflrrn = _RrnTmp;
                            if (!(bool)_IN[66])
                            {
                                if (SFSEL == 10)
                                {
                                    // Print in batch.
                                    CUSTDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array);
                                    hpNbrs = hpNbrs + 1;
                                    pNumbers[(int)(hpNbrs - 1)] = (decimal)SFCUSTNO;
                                    pTypes[(int)(hpNbrs - 1)] = "P";
                                    ClearSel();
                                }
                                else if (SFSEL == 9)
                                {
                                    // Print online.
                                    SVCUSTNO = (decimal)SFCUSTNO;
                                    _SVCUSTNOaProxy = SVCUSTNOa;
                                    _DynamicCaller.CallD("ACME.SunFarm.CUSTPRTS", out _IN.Array[88], ref _SVCUSTNOaProxy);
                                    SVCUSTNOa = _SVCUSTNOaProxy;
                                    MID = "CST0006";
                                    MTX = "";
                                    _DynamicCaller.CallD("ACME.SunFarm.MSGLOD", out _LR, ref MID, ref MTX);
                                    ClearSel();
                                }
                                else if (SFSEL == 7)
                                {
                                    // Create sales rec.
                                    CUSTDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array);
                                    hpNbrs = hpNbrs + 1;
                                    pNumbers[(int)(hpNbrs - 1)] = (decimal)SFCUSTNO;
                                    pTypes[(int)(hpNbrs - 1)] = "C";
                                    ClearSel();
                                }
                                else if (SFSEL == 5)
                                {
                                    // Display delivery
                                    CUSTDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array); //  addresses.
                                    pNumber = (decimal)SFCUSTNO;
                                    _DynamicCaller.CallD("ACME.SunFarm.CUSTDELIV", out _IN.Array[88], ref pNumber);
                                    ClearSel();

                                }
                                else if (SFSEL == 3)
                                {
                                    // Display sales and
                                    CUSTDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array); //  returns totals.
                                    SalesInfo();

                                }
                                else if (SFSEL == 11)
                                {
                                    // Maintainance.
                                    CUSTDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array);
                                    ORDCUST = (decimal)SFCUSTNO;
                                    _DynamicCaller.CallD("ACME.SunFarm.ORDHINQ", out _LR, ref ORDCUST);
                                    ClearSel();

                                }
                                else if (SFSEL == 2)
                                {
                                    // Maintainance.
                                    CUSTDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array);
                                    RcdUpdate();
                                }
                            }
                        } while (!((bool)_IN[66]));

                        if (hpNbrs > 0m)
                        {
                            //Are there any jobs
                            _DynamicCaller.CallD("ACME.SunFarm.CUSTSBMJOB", out _IN.Array[88], ref pNumbers, ref pTypes); // to submit to batch?
                            MID = "CST0005";
                            MTX = (string)hpNbrsAlf;
                            _DynamicCaller.CallD("ACME.SunFarm.MSGLOD", out _LR, ref MID, ref MTX);
                        }

                        sflrrn = savrrn;
                    }

                } while (!((bool)_IN[3]));


                //*********************************************************************
                // UPDATE THE CUSTOMER RECORD
                //*********************************************************************

            } while (!(bool)_INLR);
        }
        void RcdUpdate()
        {
            Indicator _LR = '0';
            ClearSel();
            AddUpd = "U";

            CMCUSTNO = (decimal)SFCUSTNO;
            LockRec = "N";
            CustChk();

            SFOLDNAME = CMNAME;
            hSFNAME = CMNAME;
            SFADDR1 = CMADDR1;
            SFADDR2 = CMADDR2;
            SFCITY = CMCITY;
            SFSTATE = CMSTATE;
            SFPOSTCODE = CMPOSTCODE;
            SFFAX = CMFAX;
            SFPHONE = CMPHONE;
            hsSTATUS = CMACTIVE;
            SFCONTACT = CMCONTACT;
            SFCONEMAL = CMCONEMAL;
            SFYN01 = CMYN01;

            _IN[40] = '1';
            _IN[41] = '0';
            _IN[42] = '0';
            _IN[43] = '0';
            _IN[44] = '0';
            _IN[99] = '0';

            //-------------------------------------------------------

            do
            {
                CUSTDSPF.Write("MSGSFC", _IN.Array);
                CUSTDSPF.ExFmt("CUSTREC", _IN.Array);

                _IN[40] = '0';
                _IN[41] = '0';
                _IN[42] = '0';
                _IN[43] = '0';
                _IN[44] = '0';

                ClearMsgs();
                _IN[30] = '0';

                if ((bool)_IN[3])
                {
                    break;

                }
                else if ((bool)_IN[12])
                {
                    break;

                    // Prompt
                }
                else if ((bool)_IN[4])
                {

                    if (CSRFLD == "#$STATUS")
                        CSRFLD = "SF".MoveLeft(CSRFLD);
                    if (CSRFLD == "SFSTATE" || (CSRFLD == "SFSTATUS"))
                    {
                        _DynamicCaller.CallD("ACME.SunFarm.CUSTPRMPT", out _IN.Array[88], ref CSRFLD, ref pResult);
                        if (!pResult.IsBlanks())
                        {

                            if (CSRFLD == "SFSTATE")
                                SFSTATE = (string)pResult;
                            else
                                hsSTATUS = (string)pResult;
                        }
                    }
                    else
                    {
                        MID = "CST0004";
                        MTX = "";
                        _DynamicCaller.CallD("ACME.SunFarm.MSGLOD", out _LR, ref MID, ref MTX);
                        _IN[4] = '0';
                    }

                }
                else if ((bool)_IN[6])
                {
                    SVCUSTNO = CMCUSTNO;
                    CUSTOMERL1_ReadOnly.Seek(SeekMode.SetGT, 999999999m);
                    CUSTOMERL1_ReadOnly.ReadPrevious(true); // Removed Access(*NoLock)
                    CUSTOMERL1.StatusCode = CUSTOMERL1_ReadOnly.StatusCode;
                    TEMPNO = CMCUSTNO + 100;
                    CUSTDS.Clear();
                    CMCUSTNO = TEMPNO;
                    AddUpd = "A";
                    SFCUSTNO = (decimal)TEMPNO;
                    hSFNAME = "";
                    SFADDR1 = "";
                    SFADDR2 = "";
                    SFCITY = "";
                    SFSTATE = "";
                    SFPOSTCODE = "";
                    SFFAX = 0;
                    SFPHONE = "";
                    hsSTATUS = "";
                    SFCONTACT = "";
                    SFCONEMAL = "";
                    SFYN01 = "";
                    _IN[30] = '1';

                    // Delete - leave sales hanging?

                }
                else if ((bool)_IN[11])
                {
                    LockRec = "Y";
                    CustChk();
                    if (!(bool)_IN[80])
                        CUSTOMERL1.Delete();

                    CUSTDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array);
                    SFCSZ = "";
                    SFNAME1 = "*** DELETED ***";
                    // Set the color
                    _IN[60] = '1';
                    CUSTDSPF.Update("SFL1", _IN.Array);
                    _IN[60] = '0';
                    // Delete msg
                    MID = "CST0003";
                    MTX = CMCUSTNO.MoveRight(MTX);
                    _DynamicCaller.CallD("ACME.SunFarm.MSGLOD", out _LR, ref MID, ref MTX);
                    break;

                    // Add/update on the Enter key
                }
                else
                {

                    if (AddUpd == "A")
                    {
                        EditFlds();
                        if ((bool)_IN[99])
                        {
                            // Any errors?
                            continue;
                        }
                        UpdDbFlds();
                        CUSTOMERL1.Write();

                        // Added message
                        MID = "CST0001";
                        MTX = CMCUSTNO.MoveRight(MTX);
                        _DynamicCaller.CallD("ACME.SunFarm.MSGLOD", out _LR, ref MID, ref MTX);
                        break;

                        // Update the database
                    }
                    else
                    {
                        EditFlds();
                        if ((bool)_IN[99])
                        {
                            // Any errors?
                            continue;
                        }
                        LockRec = "Y";
                        CustChk(); //Reread the record.
                        UpdDbFlds();
                        CUSTOMERL1.Update();
                        CUSTDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array);
                        SFNAME1 = CMNAME;
                        Fix_CSZ();
                        CUSTDSPF.Update("SFL1", _IN.Array);

                        // Updated message
                        MID = "CST0002";
                        MTX = CMCUSTNO.MoveLeft(MTX);
                        _DynamicCaller.CallD("ACME.SunFarm.MSGLOD", out _LR, ref MID, ref MTX);
                    }

                    // Re-open for the next update
                    LockRec = "N";
                    CustChk();
                }

            } while (!((bool)_IN[12]));

        }

        //*********************************************************************
        //  EDIT THE SCREEN FIELDS
        //*********************************************************************

        void EditFlds()
        {
            Indicator _LR = '0';
            _IN[40] = '0';
            _IN[41] = '0';
            _IN[42] = '0';
            _IN[43] = '0';
            _IN[44] = '0';
            _IN[99] = '0';

            if (hSFNAME.IsBlanks())
            {
                _IN[40] = '1';
                _IN[99] = '1';
                MID = "CST1001";
                MTX = "customer name";

            }
            else if (SFADDR1.IsBlanks())
            {
                _IN[41] = '1';
                _IN[99] = '1';
                MID = "CST1001";
                MTX = "first address line";

            }
            else if (SFCITY.IsBlanks())
            {
                _IN[42] = '1';
                _IN[99] = '1';
                MID = "CST1001";
                MTX = "city";

            }
            else if (SFSTATE.IsBlanks())
            {
                _IN[43] = '1';
                _IN[99] = '1';
                MID = "CST1002";
                MTX = "state";

            }
            else if (hsSTATUS.IsBlanks())
            {
                _IN[44] = '1';
                _IN[99] = '1';
                MID = "CST1002";
                MTX = "status";
            }

            if ((bool)_IN[99])
            {
                _DynamicCaller.CallD("ACME.SunFarm.MSGLOD", out _LR, ref MID, ref MTX);
            }

        }

        //*********************************************************************
        //  UPDATE THE DATABASE FIELDS
        //*********************************************************************

        void UpdDbFlds()
        {
            CMCUSTNO = (decimal)SFCUSTNO;
            CMNAME = ((string)hSFNAME).Trim();
            CMADDR1 = ((string)SFADDR1).Trim();
            CMADDR2 = ((string)SFADDR2).Trim();
            CMCITY = ((string)SFCITY).Trim();
            CMSTATE = SFSTATE;
            CMPOSTCODE = SFPOSTCODE;
            CMFAX = SFFAX;
            CMPHONE = SFPHONE;
            CMACTIVE = hsSTATUS;
            CMCONTACT = ((string)SFCONTACT).Trim();
            CMCONEMAL = ((string)SFCONEMAL).Trim();
            CMYN01 = ((string)SFYN01).Trim();
        }


        //*********************************************************************
        // Retrieve Calculated Sales Info
        //*********************************************************************

        void SalesInfo()
        {
            ClearSel();

            CMCUSTNO = (decimal)SFCUSTNO;
            LockRec = "N";
            CustChk();

            CmCusth = CMCUSTNO;
            Sales = 0m;
            Returns = 0m;
            CmCusthCH = CMCUSTNO.MoveRight(CmCusthCH);
            SalesCh = new string('0', 13);
            ReturnsCh = new string('0', 13);
            _DynamicCaller.CallD("ACME.SunFarm.CUSTCALC", out _IN.Array[88], ref CmCusthCH, ref SalesCh, ref ReturnsCh);
            SFNAME = CMNAME;
            SFSALES = SalesCh.ToZonedDecimal(13, 2);
            SFRETURNS = ReturnsCh.ToZonedDecimal(13, 2);

            do
            {
                CUSTDSPF.ExFmt("SALESREC", _IN.Array);
            } while (!(bool)_IN[3] && !(bool)_IN[12]);

            _IN[3] = '0';

        }


        //*********************************************************************
        //  Load Sfl Subroutine
        //*********************************************************************

        void LoadSfl()
        {
            _IN[61] = '0';
            _IN[90] = '1';
            CUSTDSPF.Write("SFLC", _IN.Array);

            _IN[76] = '0';
            _IN[90] = '0';
            sflrrn = 0m;
            _IN[77] = CUSTOMERL2.ReadNext(true) ? '0' : '1';
            if (STS02 != 0)
                SVS02 = STS02;
            else
                HRC02 = REC02;

            //----------------------------------------------------------

            while (!(bool)_IN[77] && (sflrrn < 14))
            {

                /* EOF or full s/f. */
                SFCUSTNO = (decimal)CMCUSTNO;
                SFNAME1 = CMNAME;
                Fix_CSZ();
                if ((bool)_IN[61])
                {
                    //Save the color of
                    SFCOLOR = "W";
                }
                else
                {
                    SFCOLOR = "G";
                }

                sflrrn = sflrrn + 1;
                CUSTDSPF.WriteSubfile("SFL1", (int)sflrrn, _IN.Array);
                _IN[61] = (!(bool)_IN[61]);

                _IN[77] = CUSTOMERL2.ReadNext(true) ? '0' : '1';
            }

            // Any records found?

            if (sflrrn == 0m)
            {
                sflrrn = 1;
                SFCUSTNO = 0;
                CMCUSTNO = 0m;
                SFNAME1 = "END OF FILE".MoveLeft(SFNAME1);
                SFCSZ = "";
                CUSTDSPF.WriteSubfile("SFL1", (int)sflrrn, _IN.Array);
            }

        }


        //*********************************************************************
        //  Read Backwards for a PageDown
        //*********************************************************************

        void ReadBack()
        {
            _IN[76] = '0';
            _IN[77] = '0';
            X = 0m;
            CUSTDSPF.ChainByRRN("SFL1", 1, _IN.Array); //Get the top name and
            CMNAME = SFNAME1;
            CMCUSTNO = (decimal)SFCUSTNO;
            CUSTOMERL2.Chain(true, CMNAME, CMCUSTNO);
            _IN[76] = CUSTOMERL2.ReadPrevious(true) ? '0' : '1';

            while (!(bool)_IN[76] && (X < 14))
            {
                /* EOF or full s/f. */
                X = X + 1;
                _IN[76] = CUSTOMERL2.ReadPrevious(true) ? '0' : '1';
            }

            if ((bool)_IN[76])
            {
                //Any records found?
                CUSTOMERL2.Seek(SeekMode.SetLL, new string(char.MinValue, 40));
            }

        }


        //*********************************************************************
        // Fix CSZ Subroutine
        //*********************************************************************

        void Fix_CSZ()
        {
            SFCSZ = CMCITY.TrimEnd() + ", " + CMSTATE + " " + CMPOSTCODE;
        }


        //*********************************************************************
        // CLEAR THE SELECTION NUMBER
        //*********************************************************************

        void ClearSel()
        {
            SFSEL = 0m;
            _IN[61] = (SFCOLOR == "W");
            CUSTDSPF.Update("SFL1", _IN.Array);
        }


        //*********************************************************************
        // CLEAR THE MESSAGE QUEUE
        //*********************************************************************

        void ClearMsgs()
        {
            Indicator _LR = '0';
            _DynamicCaller.CallD("ACME.SunFarm.MSGCLR", out _LR);
            MID = "";
        }



        //*********************************************************************
        //     * Init Subroutine
        //*********************************************************************
        void PROCESS_STAR_INZSR()
        {
            Indicator _LR = '0';
            CUSTOMERL2.Seek(SeekMode.SetLL, new string(char.MinValue, 40));
            LoadSfl();

            _IN[75] = '1';
            MID = "";
            aPGMQ = "*";
            _DynamicCaller.CallD("ACME.SunFarm.MSGCLR", out _LR);
        }

        //********************************************************************
        //   CHECK THE CUSTOMER NUMBER
        //********************************************************************
        // Message handling parm list
        // PLIST "#PLMSG" moved by Monarch to global scope.
        void CustChk()
        {
            if (LockRec == "N")
            {
                _IN[80] = CUSTOMERL1_ReadOnly.Chain(true, CMCUSTNO) ? '0' : '1'; // Removed Access(*NoLock)
                CUSTOMERL1.StatusCode = CUSTOMERL1_ReadOnly.StatusCode;
            }
            else
            {
                _IN[80] = CUSTOMERL1.Chain(true, CMCUSTNO) ? '0' : '1';
            }
        }

#region File Information Data Structures

        int STATUS
        {
            get => CUSTDSPF.StatusCode;
            set => CUSTDSPF.StatusCode = value;
        }

        int STS02
        {
            get => CUSTOMERL2.StatusCode;
            set => CUSTOMERL2.StatusCode = value;
        }

        int REC02 => (int)CUSTOMERL2.RecNum;

        int REC01 => (int)CUSTOMERL1.RecNum;


#endregion


        void Reset_hpNbrs()
        {
            hpNbrs = 0;
        }

#region Entry and activation methods for *ENTRY

        int _parms;

        bool _isInitialized;
        void __ENTRY()
        {
            int cparms = 0;
            bool _cleanup = true;
            try
            {
                _parms = cparms;
                if (!_isInitialized)
                {
                    PROCESS_STAR_INZSR();
                    _isInitialized = true;
                }
                StarEntry(cparms);
            }
            catch(Return)
            {
            }
            catch(System.Threading.ThreadAbortException)
            {
                _cleanup = false;
                _INLR = '1';
            }
            finally
            {
                _ = _cleanup;
            }
        }

        public static void _ENTRY(ICaller _caller, out Indicator __inLR)
        {
            __inLR = RunProgram<CUSTINQ>(_caller, (CUSTINQ _instance) => _instance.__ENTRY());
        }
#endregion

        void _instanceInit()
        {
            X = 0m;
            pTypes = new FixedStringArray<_20, _1>((FixedString<_1>[])null);

            pNumbers = new FixedDecimalArray<_20, _9, _0>((FixedDecimal<_9, _0>[])null);

            _DynamicCaller = new DynamicCaller(this);
            CUSTDSPF = new WorkstationFile(PopulateBufferCUSTDSPF, PopulateFieldsCUSTDSPF, null, "CUSTDSPF", "/SunFarmViews/CUSTDSPF");
            CUSTDSPF.Open();
            CUSTOMERL2 = new DatabaseFile(PopulateBufferCUSTOMERL2, PopulateFieldsCUSTOMERL2, null, "CUSTOMERL2", "*LIBL/CUSTOMERL2", CUSTOMERL2FormatIDs)
            { IsDefaultRFN = true };
            CUSTOMERL1 = new DatabaseFile(PopulateBufferCUSTOMERL1, PopulateFieldsCUSTOMERL1, null, "CUSTOMERL1", "*LIBL/CUSTOMERL1", CUSTOMERL1FormatIDs, blockingFactor : 0)
            { IsDefaultRFN = true };
            CUSTOMERL1_ReadOnly = new DatabaseFile(PopulateBufferCUSTOMERL1_ReadOnly, PopulateFieldsCUSTOMERL1_ReadOnly, null, "CUSTOMERL1_ReadOnly", "*LIBL/CUSTOMERL1", CUSTOMERL1_ReadOnlyFormatIDs)
            { IsDefaultRFN = true };
            CUSTDS = new (extSizeCUSTDS, CUSTDS_000, CUSTDS_001, CUSTDS_002, CUSTDS_003, CUSTDS_004, CUSTDS_005, CUSTDS_006, CUSTDS_007, 
                CUSTDS_008, CUSTDS_009, CUSTDS_010, CUSTDS_011, CUSTDS_012, CUSTDS_013, CUSTDS_014, CUSTDS_015);
        }
    }

    // /Error There are 1 NoLock Sequential Read operations. Using *NoLock on a file opened for *Update is not supported with DSS .NET
    // /Error There are 1 NoLock CHAIN operations. Using *NoLock on a file opened for *Update is not supported with DSS .NET
}
