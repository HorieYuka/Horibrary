using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate.Base
{
    class ModelSlotBase : ObservableObject
    {
        // Insert common slot properties here.

        public string SlotText { get; set; }

    }

    class ModelSlotCOM : ModelSlotBase
    {
        public EventHandler ConnectEvt;
        public EventHandler DisconnectEvt;

        public bool IsConnected { get; set; }

        public string PortNum { get; set; }
        public string PortName { get; set; }
        public int Boadrate { get; set; }

    }

    class BelowStatusModel : ObservableObject
    {
        private int? _BelowBar;
        public int? BelowBar

        {
            get => _BelowBar;
            set
            {
                _BelowBar = value;
                OnPropertyChanged("BelowBar");
            }
        }

        private string? _BelowText;
        public string? BelowText
        {
            get => _BelowText;
            set
            {
                _BelowText = value;
                OnPropertyChanged("BelowText");
            }
        }
        
        private bool? _BelowCircle;
        public bool? BelowCircle
        {
            get => _BelowCircle;
            set
            {
                _BelowCircle = value;
                OnPropertyChanged("BelowCircle");
            }
        }
    }
}
