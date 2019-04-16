using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;

namespace StashItemValuer
{
    [Cmdlet(VerbsCommon.Get, "PoeStashTabs")]
    public class GetStashTabs : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string Cookie { get; set; }

        [Parameter(Mandatory = true)]
        public string PlayerName { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(Cookie);
            WriteObject(PlayerName);
        }
    }
}
