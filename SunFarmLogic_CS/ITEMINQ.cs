﻿// Translated from Encore to C# on 10/11/2023 at 10:10:23 AM by ASNA Encore Translator® version 4.0.18.0
// ASNA Monarch(R) version 11.4.12.0 at 8/25/2023
// Migrated source location: library ERCAP, file QRPGSRC, member ITEMINQ

using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using ACME.SunFarmCustomers_Job;

using System;
using ASNA.QSys.Runtime.JobSupport;


namespace ACME.SunFarm
{
    [ActivationGroup("*DFTACTGRP"), System.ComponentModel.Description("Item Inquiry and Maintenance")]
    [ProgramEntry("_ENTRY")]
    public partial class ITEMINQ : Program
    {
        protected dynamic _DynamicCaller;

        //********************************************************************
        // INDICATORS:
        //   03     F3 pressed
        //   40-44  Cursor positioning
        //   50     PageUp pressed
        //   51     PageDown pressed
        //   66     EOF reading on the subfile
        //   76     BOF reading CUSTOMERL2
        //   77     EOF reading CUSTMOMER2
        //   88     LR seton in a called program
        //   99     General error indicator
        //********************************************************************
        WorkstationFile ITEMDSPF;
        DatabaseFile ITEMMASTL2;
        DatabaseFile ITEMMASTL1;
        DatabaseFile ITEMMASTL1_ReadOnly;
        //********************************************************************
        DataStructure _DS5 = new (3);
        FixedDecimal<_3, _0> hpNbrs { get => _DS5.GetZoned(0, 3, 0); set => _DS5.SetZoned(value, 0, 3, 0); } 

        FixedDecimalArray<_20, _9, _0> pNumbers;
        FixedString<_7> MID;
        FixedString<_30> MTX;
        FixedString<_1> LockRec;
        FixedDecimal<_5, _0> X;
        DataStructure _DS6 = new (9);
        FixedDecimal<_9, _0> SVITEMNO { get => _DS6.GetZoned(0, 9, 0); set => _DS6.SetZoned(value, 0, 9, 0); } 

        // ITEM       DS
        DataStructure ITEMDS;

        DataStructure _DS7 = new (429);
        FixedDecimal<_3, _0> NbrOfparms { get => _DS7.GetZoned(35, 3, 0); set => _DS7.SetZoned(value, 35, 3, 0); } 

        //********************************************************************
        // Fields defined in main C-Specs declared now as Global fields (Monarch generated)
        FixedString<_1> AddUpd;
        FixedString<_20> Name20;
        FixedDecimal<_9, _0> pRtnItem;
        FixedString<_1> pSelOnly;
        FixedDecimal<_4, _0> savrrn;
        FixedDecimal<_4, _0> sflrrn;
        FixedDecimal<_9, _0> TEMPNO;

        // PLIST(s) relocated by Monarch
        
        // KLIST(s) relocated by Monarch
        
#region Constructor and Dispose 
        public ITEMINQ()
        {
            _instanceInit();
            // Initialization of Data Structure fields (Monarch generated)
            Reset_hpNbrs();

            ITEMMASTL1.Open(CurrentJob.Database, AccessMode.RWCD, false, false, ServerCursors.Default);
            ITEMMASTL1_ReadOnly.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            ITEMMASTL2.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
        }

        override public void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                ITEMDSPF.Close();
                ITEMMASTL2.Close();
                ITEMMASTL1.Close();
                ITEMMASTL1_ReadOnly.Close();
            }
            base.Dispose(disposing);
        }


#endregion
        void StarEntry(int cparms)
        {
            int _RrnTmp = 0;
            do
            {
                NbrOfparms = _parms;

                //********************************************************************
                // KLIST "KeyMastL2" moved by Monarch to global scope.
                //********************************************************************
                if (NbrOfparms > 1)
                    pRtnItem = 0m;
                //********************************************************************
                do
                {
                    _IN[90] = '0';
                    ITEMDSPF.Write("MSGSFC", _IN.Array);
                    ITEMDSPF.Write("KEYS", _IN.Array);
                    ITEMDSPF.ExFmt("SFLC", _IN.Array);
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
                        ITEMMASTL2.Seek(SeekMode.SetLL, ITEMSHRTDS);
                        LoadSfl();
                    }
                    else if (!SETITMDSC.IsBlanks())
                    {
                        Name20 = SETITMDSC;
                        ITEMMASTL2.Seek(SeekMode.SetLL, Name20);
                        LoadSfl();
                        // PageDown-RollUp
                    }
                    else if ((bool)_IN[51])
                    {
                        ReadBack();
                        LoadSfl();
                    }
                    else if (!SETITMDSC.IsBlanks())
                    {
                        Name20 = SETITMDSC;
                        ITEMMASTL2.Seek(SeekMode.SetLL, Name20);
                        LoadSfl();
                    }
                    else
                    {
                        _IN[30] = '0';
                        _IN[66] = '0';
                        savrrn = sflrrn;
                        pNumbers.Initialize(0m);
                        hpNbrs = 0m;
                        do
                        {
                            _RrnTmp = (int)sflrrn;
                            _IN[66] = ITEMDSPF.ReadNextChanged("SFL1", ref _RrnTmp, _IN.Array) ? '0' : '1';
                            sflrrn = _RrnTmp;
                            if ((!(bool)_IN[66]) && SFSEL == 1)
                            {
                                if (NbrOfparms > 1 && pSelOnly == "Y")
                                {
                                    // Maintainance.
                                    // If only selecting,
                                    pRtnItem = SFITEMNUM;
                                    _IN[3] = '1';
                                    break;
                                }
                                RcdUpdate();
                            }
                        } while (!((bool)_IN[66]));
                        sflrrn = savrrn;
                    }
                } while (!((bool)_IN[3]));
                _INLR = '1';
                //*********************************************************************
                // UPDATE THE Item RECORD
                //*********************************************************************
            } while (!(bool)_INLR);
        }
        void RcdUpdate()
        {
            Indicator _LR = '0';
            ClearSel();
            AddUpd = "U";
            ITEMNUMBER = SFITEMNUM;
            LockRec = "N";
            ItemChk();
            SFITEM = ITEMNUMBER;
            SFLNGDESC = ITEMDESC;
            SFOLDDESC = ITEMSHRTDS;
            SFDESC = ITEMSHRTDS;
            SFPRICE = ITEMPRICE;
            SFWEIGHT = ITEMWEIGHT;
            SFAVAIL = ITEMQTYAVL;
            SFONORDR = ITEMQTYORD;
            _IN[40] = '1';
            _IN[41] = '0';
            _IN[42] = '0';
            _IN[43] = '0';
            _IN[44] = '0';
            _IN[99] = '0';
            //-------------------------------------------------------
            do
            {
                ITEMDSPF.Write("MSGSFC", _IN.Array);
                ITEMDSPF.ExFmt("ITEMREC", _IN.Array);
                _IN[40] = '0';
                _IN[41] = '0';
                _IN[42] = '0';
                _IN[43] = '0';
                _IN[44] = '0';
                ClearMsgs();
                _IN[30] = '0';
                if ((bool)_IN[3])
                    break;
                else if ((bool)_IN[12])
                    break;
                else if ((bool)_IN[6])
                {
                    SVITEMNO = ITEMNUMBER;
                    ITEMMASTL1_ReadOnly.Seek(SeekMode.SetGT, 999999999m);
                    ITEMMASTL1_ReadOnly.ReadPrevious(true); // Removed Access(*NoLock)
                    ITEMMASTL1.StatusCode = ITEMMASTL1_ReadOnly.StatusCode;
                    TEMPNO = ITEMNUMBER + 100;
                    ITEMDS.Clear();
                    ITEMNUMBER = TEMPNO;
                    AddUpd = "A";
                    SFITEM = TEMPNO;
                    SFLNGDESC = "";
                    SFDESC = "";
                    SFPRICE = 0;
                    SFUNIT = "";
                    SFWEIGHT = 0m;
                    SFAVAIL = 0;
                    SFONORDR = 0;
                    _IN[30] = '1';
                    // Delete - leave sales hanging?
                }
                else if ((bool)_IN[11])
                {
                    LockRec = "Y";
                    ItemChk();
                    if (!(bool)_IN[80])
                        ITEMMASTL1.Delete();
                    ITEMDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array);
                    SFITMPRIC = 0;
                    SFITMAVAIL = 0;
                    SFITEMDESC = "*** DELETED ***";
                    // Set the color
                    _IN[60] = '1';
                    ITEMDSPF.Update("SFL1", _IN.Array);
                    _IN[60] = '0';
                    // Delete msg
                    MID = "ITM0003";
                    MTX = ITEMNUMBER.MoveRight(MTX);
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
                        ITEMMASTL1.Write();
                        ITEMMASTL2.Seek(SeekMode.SetLL, new string(char.MinValue, 20));
                        LoadSfl();
                        // Added message
                        MID = "ITM0001";
                        MTX = SFITEM.MoveRight(MTX);
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
                        ItemChk(); //Reread the record.
                        UpdDbFlds();
                        ITEMMASTL1.Update();
                        ITEMDSPF.ChainByRRN("SFL1", (int)sflrrn, _IN.Array);
                        SFITEMNUM = ITEMNUMBER;
                        SFITEMDESC = ITEMSHRTDS;
                        SFITMPRIC = ITEMPRICE;
                        SFITMAVAIL = ITEMQTYAVL;
                        ITEMDSPF.Update("SFL1", _IN.Array);
                        // Updated message
                        MID = "ITM0002";
                        MTX = ITEMNUMBER.MoveLeft(MTX);
                        _DynamicCaller.CallD("ACME.SunFarm.MSGLOD", out _LR, ref MID, ref MTX);
                    }
                    // Re-open for the next update
                    LockRec = "N";
                    ItemChk();
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
            if (SFLNGDESC.IsBlanks())
            {
                _IN[40] = '1';
                _IN[99] = '1';
                MID = "ITM0004";
                MTX = "Long description";
            }
            else if (SFDESC.IsBlanks())
            {
                _IN[41] = '1';
                _IN[99] = '1';
                MID = "ITM0004";
                MTX = "Description";
            }
            else if (SFPRICE == 0)
            {
                _IN[42] = '1';
                _IN[99] = '1';
                MID = "ITM0005";
                MTX = "Price";
            }
            else if (SFUNIT.IsBlanks())
            {
                _IN[43] = '1';
                _IN[99] = '1';
                MID = "ITM0004";
                MTX = "Unit of Measure";
            }
            else if (SFWEIGHT == 0m)
            {
                _IN[44] = '1';
                _IN[99] = '1';
                MID = "ITM0005";
                MTX = "Weight";
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
            ITEMNUMBER = SFITEM;
            ITEMDESC = SFLNGDESC;
            ITEMSHRTDS = SFDESC;
            ITEMPRICE = SFPRICE;
            ITEMUNIT = SFUNIT;
            ITEMWEIGHT = SFWEIGHT;
            ITEMQTYAVL = SFAVAIL;
            ITEMQTYORD = SFONORDR;
        }
        //********************************************************************
        //   CHECK THE ITEM NUMBER
        //********************************************************************
        void ItemChk()
        {
            if (LockRec == "N")
            {
                _IN[80] = ITEMMASTL1_ReadOnly.Chain(true, ITEMNUMBER) ? '0' : '1'; // Removed Access(*NoLock)
                ITEMMASTL1.StatusCode = ITEMMASTL1_ReadOnly.StatusCode;
            }
            else
            {
                _IN[80] = ITEMMASTL1.Chain(true, ITEMNUMBER) ? '0' : '1';
            }
        }
        //*********************************************************************
        //  Load Sfl Subroutine
        //*********************************************************************
        void LoadSfl()
        {
            _IN[61] = '0';
            _IN[90] = '1';
            ITEMDSPF.Write("SFLC", _IN.Array);
            _IN[76] = '0';
            _IN[90] = '0';
            sflrrn = 0m;
            _IN[77] = ITEMMASTL2.ReadNext(true) ? '0' : '1';
            //----------------------------------------------------------
            while (!(bool)_IN[77] && (sflrrn < 14))
            {
                /* EOF or full s/f. */
                SFITEMNUM = ITEMNUMBER;
                SFITEMDESC = ITEMSHRTDS;
                SFITMPRIC = ITEMPRICE;
                SFITMAVAIL = ITEMQTYAVL;
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
                ITEMDSPF.WriteSubfile("SFL1", (int)sflrrn, _IN.Array);
                _IN[61] = (!(bool)_IN[61]);
                _IN[77] = ITEMMASTL2.ReadNext(true) ? '0' : '1';
            }
            // Any records found?
            if (sflrrn == 0m)
            {
                sflrrn = 1;
                SFITEMNUM = 0;
                ITEMNUMBER = 0m;
                SFITEMDESC = "END OF FILE";
                SFITMPRIC = 0;
                SFITMAVAIL = 0;
                ITEMDSPF.WriteSubfile("SFL1", (int)sflrrn, _IN.Array);
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
            ITEMDSPF.ChainByRRN("SFL1", 1, _IN.Array); //Get the top name and
            ITEMSHRTDS = SFITEMDESC;
            ITEMNUMBER = SFITEMNUM;
            ITEMMASTL2.Chain(true, ITEMSHRTDS, ITEMNUMBER);
            _IN[76] = ITEMMASTL2.ReadPrevious(true) ? '0' : '1';
            while (!(bool)_IN[76] && (X < 14))
            {
                /* EOF or full s/f. */
                X = X + 1;
                _IN[76] = ITEMMASTL2.ReadPrevious(true) ? '0' : '1';
            }
            if ((bool)_IN[76])
            {
                //Any records found?
                ITEMMASTL2.Seek(SeekMode.SetLL, new string(char.MinValue, 20));
            }
        }
        //*********************************************************************
        // CLEAR THE SELECTION NUMBER
        //*********************************************************************
        void ClearSel()
        {
            SFSEL = 0m;
            _IN[61] = (SFCOLOR == "W");
            ITEMDSPF.Update("SFL1", _IN.Array);
        }
        //*********************************************************************
        // CLEAR THE MESSAGE QUEUE
        //*********************************************************************
        void ClearMsgs()
        {
            Indicator _LR = '0';
            _DynamicCaller.CallD("ACME.SunFarm.MSGCLR", out _LR);
        }
        //*********************************************************************
        // Init Subroutine
        //*********************************************************************
        void PROCESS_STAR_INZSR()
        {
            Indicator _LR = '0';
            ITEMMASTL2.Seek(SeekMode.SetLL, new string(char.MinValue, 20));
            LoadSfl();
            _IN[75] = '1';
            MID = "";
            aPGMQ = "*";
            _DynamicCaller.CallD("ACME.SunFarm.MSGCLR", out _LR);
        }

        // Message handling parm list
        // PLIST "#PLMSG" moved by Monarch to global scope.
        void Reset_hpNbrs()
        {
            hpNbrs = 0;
        }

#region Entry and activation methods for *ENTRY

        int _parms;

        bool _isInitialized;
        void __ENTRY(ref FixedString<_1> _pSelOnly, ref FixedDecimal<_9, _0> _pRtnItem)
        {
            int cparms = 2;
            bool _cleanup = true;
            pRtnItem = _pRtnItem;
            pSelOnly = _pSelOnly;
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
                if (_cleanup)
                {
                    _pSelOnly = pSelOnly;
                    _pRtnItem = pRtnItem;
                }
            }
        }

        public static void _ENTRY(ICaller _caller, out Indicator __inLR, ref FixedString<_1> pSelOnly, ref FixedDecimal<_9, _0> pRtnItem)
        {
            FixedString<_1> _ppSelOnly = pSelOnly;
            FixedDecimal<_9, _0> _ppRtnItem = pRtnItem;
            __inLR = RunProgram<ITEMINQ>(_caller, (ITEMINQ _instance) => _instance.__ENTRY(ref _ppSelOnly, ref _ppRtnItem));
            pSelOnly = _ppSelOnly;
            pRtnItem = _ppRtnItem;
        }
#endregion

        void _instanceInit()
        {
            X = 0m;
            pNumbers = new FixedDecimalArray<_20, _9, _0>((FixedDecimal<_9, _0>[])null);

            _DynamicCaller = new DynamicCaller(this);
            ITEMDSPF = new WorkstationFile(PopulateBufferITEMDSPF, PopulateFieldsITEMDSPF, null, "ITEMDSPF", "/SunFarmViews/ITEMDSPF");
            ITEMDSPF.Open();
            ITEMMASTL2 = new DatabaseFile(PopulateBufferITEMMASTL2, PopulateFieldsITEMMASTL2, null, "ITEMMASTL2", "*LIBL/ITEMMASTL2", ITEMMASTL2FormatIDs)
            { IsDefaultRFN = true };
            ITEMMASTL1 = new DatabaseFile(PopulateBufferITEMMASTL1, PopulateFieldsITEMMASTL1, null, "ITEMMASTL1", "*LIBL/ITEMMASTL1", ITEMMASTL1FormatIDs, blockingFactor : 0)
            { IsDefaultRFN = true };
            ITEMMASTL1_ReadOnly = new DatabaseFile(PopulateBufferITEMMASTL1_ReadOnly, PopulateFieldsITEMMASTL1_ReadOnly, null, "ITEMMASTL1_ReadOnly", "*LIBL/ITEMMASTL1", ITEMMASTL1_ReadOnlyFormatIDs)
            { IsDefaultRFN = true };
            ITEMDS = new (extSizeITEMDS, ITEMDS_000, ITEMDS_001, ITEMDS_002, ITEMDS_003, ITEMDS_004, ITEMDS_005, ITEMDS_006, ITEMDS_007, 
                ITEMDS_008);
        }
    }

    // /Error There are 1 NoLock Sequential Read operations. Using *NoLock on a file opened for *Update is not supported with DSS .NET
    // /Error There are 1 NoLock CHAIN operations. Using *NoLock on a file opened for *Update is not supported with DSS .NET
}
