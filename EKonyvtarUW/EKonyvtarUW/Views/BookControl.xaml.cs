using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EKonyvtarUW.Views
{
    public sealed partial class BookControl : UserControl
    {

        //private Compositor _compositor;

        public BookControl()
        {
            this.InitializeComponent();
            //_compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            //Loaded += BookControl_Loaded; ;
        }
    }
}
