using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TimeTracker
{
    public partial class Job_Overview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gridViewJobTrack_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell thc = new TableHeaderCell();
                thc.ColumnSpan = 2;
                thc.Text = "Header 1";
                gvr.Cells.Add(thc);

                thc = new TableHeaderCell();
                thc.ColumnSpan = 3;
                thc.Text = "Header 3";
                gvr.Cells.Add(thc);

                thc = new TableHeaderCell();
                thc.ColumnSpan = 4;
                thc.Text = "Header 2";
                gvr.Cells.Add(thc);

                gridJobTrack.Controls[0].Controls.AddAt(0, gvr);
            }
        }
    }
}