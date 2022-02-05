using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace LCUTest.Model
{
    public class Log : BindableBase
    {
        private int _id;
        private string _description;
        private string _time;

        public int Id { get => _id; set => SetProperty(ref _id, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string Time { get => _time; set => SetProperty(ref _time, value); }
    }
}
