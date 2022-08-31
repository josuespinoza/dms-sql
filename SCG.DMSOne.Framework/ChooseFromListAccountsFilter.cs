using System;
using System.Collections.Generic;
using SAPbouiCOM;

namespace SCG.DMSOne.Framework
{
    /// <summary>
    /// Class for applying filters to the Accounts ChooseFromList.
    /// It will filter by AcctCode
    /// </summary>
    public class ChooseFromListAccountsFilter
    {
        private readonly IChooseFromListEvent _chooseFromListEvent;
        private readonly Application _application;
        private readonly List<string> _accountCodes;

        /// <summary>
        /// Creates an instance of ChooseFromListAccountsFilter
        /// </summary>
        /// <param name="accountCodes">List of account codes that will be shown</param>
        /// <param name="chooseFromListEvent">ChooseFromListEvent from the ItemEvent (pval)</param>
        /// <param name="application">UI Api Application object</param>
        public ChooseFromListAccountsFilter(List<string> accountCodes, IChooseFromListEvent chooseFromListEvent,
                                           Application application)
        {
            if (!chooseFromListEvent.BeforeAction)
                throw new InvalidOperationException("This filters can only be applied on the BeforeAction event");
            _accountCodes = accountCodes;
            _chooseFromListEvent = chooseFromListEvent;
            _application = application;
        }

        public Application Application
        {
            get { return _application; }
        }

        public List<string> AccountCodes
        {
            get { return _accountCodes; }
        }

        public IChooseFromListEvent ChooseFromListEvent
        {
            get { return _chooseFromListEvent; }
        }

        /// <summary>
        /// This methos will apply the account codes filter to the current ChooseFromList
        /// </summary>
        public void ApplyFilter()
        {
            if (AccountCodes != null)
            {
                Form form = Application.Forms.GetForm(ChooseFromListEvent.FormTypeEx, ChooseFromListEvent.FormTypeCount);
                var chooseFromList =
                    form.ChooseFromLists.Item(ChooseFromListEvent.ChooseFromListUID);
                var conditions = (Conditions)Application.CreateObject(BoCreatableObjectType.cot_Conditions);

                for (int index = 0; index < AccountCodes.Count; index++)
                {
                    var accountCode = AccountCodes[index];
                    var condition = conditions.Add();
                    if (index == 0)
                        condition.BracketOpenNum = 2;
                    else
                        condition.BracketOpenNum = 1;
                    condition.Alias = "AcctCode";
                    condition.Operation = BoConditionOperation.co_EQUAL;
                    condition.CondVal = accountCode;
                    if (index == AccountCodes.Count - 1)
                        condition.BracketCloseNum = 2;
                    else
                    {
                        condition.BracketCloseNum = 1;
                        condition.Relationship = BoConditionRelationship.cr_OR;
                    }
                }
                chooseFromList.SetConditions(conditions);
            }
        }
    }
}