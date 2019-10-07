using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5k.UI
{
    public class ViewModel : INotifyPropertyChanged
    {
        /*
        public string Input { get; set; }

        private string _output = string.Empty;
        public string Output
        {
            get { return _output; }
            set
            {
                if (_output != value)
                {
                    _output = value;
                    OnPropertyChanged("Output");
                }
            }
        }
        */
        public DelegateCommand SayHelloCommand { get; private set; }

        public ViewModel()
        {
            SayHelloCommand = new DelegateCommand(SayHello);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SayHello(object parameter)
        {
            Console.WriteLine("hello?");
            //string param = (string)parameter;
            //Output = System.String.Format("Hello, {0} ({1})", Input, param);
        }
    }
}
