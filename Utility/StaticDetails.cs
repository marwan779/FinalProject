namespace InventoryManagementSystem.Utility
{
    public static class StaticDetails
    {
        public const string OwnerRole = "Owner";
        public const string CustomerRole = "Customer";
        public const string SupplierRole = "Supplier";

        public static List<string> allowedImageExtensions = new List<string> { ".jpg", ".jpeg", ".png" };

        public const string PurchaseOrderPending = "Pending";
        public const string PurchaseOrderShipped = "Shipped";
        public const string PurchaseOrderRejected = "Rejected";
        public const string PurchaseOrderDelivered = "Delivered ";

    }
}
