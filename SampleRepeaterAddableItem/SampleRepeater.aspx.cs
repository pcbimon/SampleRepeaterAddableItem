using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace SampleRepeaterAddableItem
{
    public partial class SampleRepeater : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitItemDt();
            }
        }

        protected void btnAddItem_OnClick(object sender, EventArgs e)
        {
            AddItem();
            BindItem();
        }

        private void InitItemDt()
        {
            DataTable itemDt = new DataTable();
            itemDt.Columns.Add("ItemName", typeof(string));
            itemDt.Columns.Add("Image", typeof(Stream));
            itemDt.Columns.Add("ImageName", typeof(string));
            itemDt.Columns.Add("ImageType", typeof(string));
            itemDt.Columns.Add("ImageContentLength", typeof(string));


            //store to session
            Session["ItemDt"] = itemDt;
        }

        private void BindItem()
        {
            // get data from session
            DataTable itemDt = (DataTable)Session["ItemDt"];
            // bind to repeater
            rpSample.DataSource = itemDt;
            rpSample.DataBind();
        }

        private void AddItem()
        {
            // Get the current data from Session
            DataTable itemDt = (DataTable)Session["ItemDt"];
            
            // get added item from repeater
            foreach (RepeaterItem repeaterItem in rpSample.Items)
            {
                // get control from repeater
                TextBox txtItemName = (TextBox)repeaterItem.FindControl("tbName");
                FileUpload fuImage = (FileUpload)repeaterItem.FindControl("fuItem");
                // check if fileupload has file
                if (fuImage.HasFile)
                {
                    // get file name
                    string fileName = fuImage.FileName;
                    // get file type
                    string fileType = fuImage.PostedFile.ContentType;
                    // get file stream
                    Stream fileStream = fuImage.PostedFile.InputStream;
                    // update file date to datatable
                    MemoryStream ms = new MemoryStream();
                    fileStream.CopyTo(ms);
                    itemDt.Rows[repeaterItem.ItemIndex]["Image"] = ms;
                    itemDt.Rows[repeaterItem.ItemIndex]["ImageName"] = fileName;
                    itemDt.Rows[repeaterItem.ItemIndex]["ImageType"] = fileType;
                    itemDt.Rows[repeaterItem.ItemIndex]["ImageContentLength"] = fuImage.PostedFile.ContentLength;
                }
                itemDt.Rows[repeaterItem.ItemIndex]["ItemName"] = txtItemName.Text;
            }
            // Add new row
            itemDt.Rows.Add("", null, null, null);
            //update session
            Session["ItemDt"] = itemDt;
        }

        protected void rpSample_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // get control from repeater
                TextBox txtItemName = (TextBox)e.Item.FindControl("tbName");
                // get data from session
                DataTable temp = (DataTable)Session["ItemDt"];
                // copy to new Datatable name itemDt
                DataTable itemDt = temp.Copy();
                // set value to control
                txtItemName.Text = itemDt.Rows[e.Item.ItemIndex]["ItemName"].ToString();
                // check if image is not null
                if (itemDt.Rows[e.Item.ItemIndex]["Image"] != DBNull.Value)
                {
                    // set image to previewImage
                    Image previewImage = (Image)e.Item.FindControl("previewImage");
                    // get image stream
                    MemoryStream imageStream = (MemoryStream)itemDt.Rows[e.Item.ItemIndex]["Image"];
                    if (imageStream != null && imageStream.CanRead)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            imageStream.Position = 0;
                            imageStream.CopyTo(memoryStream);
                            string base64String = Convert.ToBase64String(memoryStream.ToArray());
                            previewImage.ImageUrl = "data:" + itemDt.Rows[e.Item.ItemIndex]["ImageType"] + ";base64," + base64String;
                        }
                    }
                    
                }
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            // get data from session
            DataTable itemDt = (DataTable)Session["ItemDt"];
            Console.WriteLine(itemDt.Rows.Count);
        }
    }
}