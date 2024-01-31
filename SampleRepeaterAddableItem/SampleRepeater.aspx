<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Site.Master" CodeBehind="SampleRepeater.aspx.cs" Inherits="SampleRepeaterAddableItem.SampleRepeater" %>


<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <h1>List my items</h1>
    <asp:UpdatePanel runat="server" ClientIDMode="AutoID" ID="updItem" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Repeater runat="server" ID="rpSample" OnItemDataBound="rpSample_OnItemDataBound">
                <ItemTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Image width="300px" ID="previewImage" runat="server"/>
                            <label class="btn btn-primary">
                            File Upload
                            <asp:FileUpload runat="server" ID="fuItem" hidden onchange="onSelectedFile(this)" accept="image/*" />
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="tbName" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="row mt-2">
                <asp:Button runat="server" ID="btnAddItem" CssClass="btn btn-primary" Text="Add Item" OnClick="btnAddItem_OnClick"/>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="row mt-2">
        <div class="col-md-12">
            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" OnClick="btnSave_OnClick" Text="Save"/>
        </div>
    </div>
    <script>
        function onSelectedFile(input) {
            var imgPreview = input.parentElement.parentElement.querySelector('img');
            //check is file selected is image
            if (input.files && input.files[0] && input.files[0].type.match('image.*')) {
                var reader = new FileReader();
                reader.onload = function(e) {
                    //set image source
                    imgPreview.src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

    </script>
</asp:Content>
