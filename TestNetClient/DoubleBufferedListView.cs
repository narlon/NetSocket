using System.Windows.Forms;

namespace TestNetClient
{
    public class DoubleBufferedListView : ListView
    {
        public DoubleBufferedListView()
        {
            this.DoubleBuffered = true;
        }
    }
}
