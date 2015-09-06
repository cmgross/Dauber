using System;
using System.Text;

namespace Dauber.Models
{
    public class ClientsTutorialViewModel
    {
        const string MfpHead = @"=IMPORTDATA(CONCATENATE(""https://dauber.apphb.com/api/v1/diary/";
        const string FitoHead = @"=IMPORTDATA(CONCATENATE(""https://dauber.apphb.com/api/v1/fitocracy/";
        const string MfpTail = @"/"",$A$1,""/"",TEXT(A2,""mm-DD-yyyy"")))";
        const string FitoTail = @"/"",$A3,""/"",TEXT($B$2,""mm-DD-yyyy"")))";
        public string MfpFormula { get; set; }
        public string FitoFormula { get; set; }
        public DAL.Coach Coach { get; set; }

        public ClientsTutorialViewModel() { }

        public ClientsTutorialViewModel(string username)
        {
            Coach = DAL.Coach.Get(username);
            MfpFormula = GetFormula(MfpHead, MfpTail);
            FitoFormula = GetFormula(FitoHead, FitoTail); 
        }

        private string GetFormula(string head, string tail)
        {
            var formula = new StringBuilder(head);
            formula.Append(Coach.CoachId);
            formula.Append(tail);
            return formula.ToString();
        }
    }
}