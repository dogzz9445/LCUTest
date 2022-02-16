using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace LCUTest.ViewModel
{
    public class RecordViewModel : BindableBase
    {
        private bool _isConncted;

        public bool IsConncted { get => _isConncted; set => SetProperty(ref _isConncted, value); }
    }
}
