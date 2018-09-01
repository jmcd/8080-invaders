using System.Windows.Forms;

namespace WindowsFormsApp
{
    public static class ToolStripDropDownMenuConvenience
    {
        public static void CheckAndUncheckSiblings(this ToolStripMenuItem toolStripMenuItem)
        {
            var parent = (ToolStripDropDownMenu) toolStripMenuItem.GetCurrentParent();
            foreach (ToolStripMenuItem sibling in parent.Items)
            {
                sibling.CheckState = sibling == toolStripMenuItem ? CheckState.Checked : CheckState.Unchecked;
            }
        }
    }
}