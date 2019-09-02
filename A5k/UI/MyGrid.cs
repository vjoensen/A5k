using Noesis;


namespace A5k.UI
{
    public class MyGrid : Grid
    {
        public MyGrid()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Noesis.GUI.LoadComponent(this, "UI\\UItest0.xaml");
        }


        protected override bool ConnectEvent(object source, string eventName, string handlerName)
        {
            System.Console.WriteLine("EventHookup?");
            if (eventName == "Click" && handlerName == "OnButton1Click")
            {
                ((Button)source).Click += this.OnButton1Click;
                return true;
            }

            if (eventName == "Click" && handlerName == "OnButton2Click")
            {
                ((Button)source).Click += this.OnButton2Click;
                return true;
            }

            return false;
        }

        private void OnButton1Click(object sender, RoutedEventArgs args)
        {
            System.Console.WriteLine("Button1 was clicked");
        }

        private void OnButton2Click(object sender, RoutedEventArgs args)
        {
            System.Console.WriteLine("Button2 was clicked");
        }
    }
}
