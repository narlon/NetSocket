using System.Windows.Forms;

namespace NetDebugger
{
    public class DoubleBufferedListView : ListView
    {
        public DoubleBufferedListView()
        {
            this.DoubleBuffered = true;
        }
    }
}
