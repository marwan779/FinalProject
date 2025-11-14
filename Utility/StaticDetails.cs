namespace InventoryManagementSystem.Utility
{
    public static class StaticDetails
    {

        /*======================== Roles ========================*/

        public const string OwnerRole = "Owner";
        public const string CustomerRole = "Customer";
        public const string SupplierRole = "Supplier";

        /*======================== Image Extensions ========================*/

        public static List<string> allowedImageExtensions = new List<string> { ".jpg", ".jpeg", ".png" };


        /*======================== Order Statuses ========================*/

        public const string PurchaseOrderPending = "Pending";
        public const string PurchaseOrderShipped = "Shipped";
        public const string PurchaseOrderRejected = "Rejected";
        public const string PurchaseOrderDelivered = "Delivered ";

        /*======================== Transaction Types ========================*/

        public const string SaleTransaction = "Sale Transaction";
        public const string PurchaseTransaction = "Purchase Transaction";



    }
}
