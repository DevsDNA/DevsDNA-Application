namespace DevsDNA.Application.Controls
{
    using System.Windows.Input;
    using Xamarin.Forms;

    public partial class ButtonRoundedGradient : FrameGradient
    {
        public static BindableProperty TextProperty = BindableProperty.Create(nameof(TextProperty), typeof(string), typeof(ButtonRoundedGradient), null, propertyChanged: OnTextChanged);
        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ButtonRoundedGradient), null);
        
     
        public ButtonRoundedGradient()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ButtonRoundedGradient buttonRoundedGradient)
            {
                buttonRoundedGradient.LblText.Text = newValue?.ToString();
            }
        }

    }
}