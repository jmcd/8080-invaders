using System;
using System.Windows.Forms;
using SpaceInvaders;

namespace WindowsFormsApp
{
    public partial class Form
    {
        private void SetupMenus()
        {
            foreach (var targetSpeedValue in new[] {0.1, 0.5, 1, 2, 5, 10})
            {
                var menuItem = new ToolStripMenuItem($"{targetSpeedValue:0.0} x", null, (sender, args) =>
                {
                    targetSpeed = new Speed(targetSpeedValue);
                    ((ToolStripMenuItem) sender).CheckAndUncheckSiblings();
                })
                {
                    CheckState = Math.Abs(targetSpeed.Value - targetSpeedValue) < 0.001 ? CheckState.Checked : CheckState.Unchecked
                };
                targetSpeedToolStripMenuItem.DropDownItems.Add(menuItem);
            }

            foreach (var colorTable in new[]
            {
                ("Mono", ColorTable.Mono),("Green Lower Third", ColorTable.GreenLowerThird),("Rainbow",ColorTable.Rainbow)
            })
            {
                var menuItem = new ToolStripMenuItem(colorTable.Item1, null, (sender, args) =>
                {
                    ColorTable = colorTable.Item2;
                    ((ToolStripMenuItem) sender).CheckAndUncheckSiblings();
                })
                {
                    CheckState = ColorTable==colorTable.Item2 ? CheckState.Checked : CheckState.Unchecked
                };
                cellophaneToolStripMenuItem.DropDownItems.Add(menuItem);
            }

            for (var dips = 0; dips < 4; dips++)
            {
                var shipCount = 3 + dips;
                var menuItem = new ToolStripMenuItem($"{shipCount} ({Convert.ToString(dips, 2).PadLeft(2, '0')})", null, (sender, args) =>
                {
                    machine.Inputs.SetShipCount(shipCount);
                    ((ToolStripMenuItem) sender).CheckAndUncheckSiblings();
                })
                {
                    CheckState = shipCount == machine.Inputs.GetShipCount() ? CheckState.Checked : CheckState.Unchecked
                };
                shipsToolStripMenuItem.DropDownItems.Add(menuItem);
            }

            foreach (var score in new[] {1000, 1500})
            {
                var menuItem = new ToolStripMenuItem($"{score}", null, (sender, args) =>
                {
                    machine.Inputs.SetExtraShipScore(score);
                    ((ToolStripMenuItem) sender).CheckAndUncheckSiblings();
                })
                {
                    CheckState = score == machine.Inputs.GetExtraShipScore() ? CheckState.Checked : CheckState.Unchecked
                };
                extraShipScoreDIPSwitchOnBit3OfInputPort2ToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }
    }
}