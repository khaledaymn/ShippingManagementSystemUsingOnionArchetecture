using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using ShippingManagementSystem.Domain.DTOs.ProductDTOs;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.OrderSpecification;
using ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.OrdersExamples.Requests;
using ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Error;
using ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Success;
using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Get All Orders

        [HttpGet]
        [Route("~/Orders/GetAll")]
        [Authorize(Policy =
          $"Permission={Orders.View};" +
          $"RequiredRole={Roles.Employee};" +
          $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "GetOrdersList",
            Summary = "Retrieve a paginated list of shipments with advanced filtering",
            Description = "### 📦 Order Discovery & Intelligence\n\n" +
                        "This endpoint serves as the primary data source for order management. It allows users to browse through the entire shipment ledger using a powerful multi-criteria filtering engine.\n\n" +
                        "#### **⚙️ Search & Filter Capabilities:**\n" +
                        "| Component | Logic |\n" +
                        "| :--- | :--- |\n" +
                        "| **Global Search** | Matches query against `CustomerName`, `Phone1`, and `Phone2`. |\n" +
                        "| **Status Filter** | Isolates orders by their current workflow state (e.g., `New`, `PostPoned`). |\n" +
                        "| **Temporal Range** | Filters by `FromDate` and `ToDate` to analyze specific timeframes. |\n" +
                        "| **Entity Binding** | Filters specifically by `MerchantId` or `ShippingRepresentativeId`. |\n\n" +
                        "#### **🔒 Internal Execution Workflow:**\n" +
                        "1. **Parameter Binding**: Captures all criteria from the query string into the `OrderParams` object. \n" +
                        "2. **Specification Building**: The `OrderSpecification` class dynamically constructs the SQL `WHERE` clause based on provided non-null parameters. \n" +
                        "3. **Data Fetching**: Executes a repository call with **Eager Loading** for `City`, `Merchant`, and `Representative` navigation properties. \n" +
                        "4. **Pagination Wrapper**: Calculates total counts and wraps results in a `PaginationResponse` for consistent UI handling. \n\n" +
                        "> **Role Note:** Administrators can view all records, while Merchants and Representatives see data filtered by their respective ownership.",
            Tags = new[] { "2. Operations & Order Lifecycle" }
        )]
        [SwaggerRequestExample(typeof(OrderParams), typeof(OrderParamsExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllOrdersSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalErrorExample))]
        [ProducesResponseType(typeof(PaginationResponse<OrderDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderParams param)
        {
            try
            {
                var result = await _unitOfWork.OrderService.GetAllOrdersAsync(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Get Order By Id

        [HttpGet]
        [Route("~/Orders/GetById/{id}")]
        [Authorize(Policy =
          $"Permission={Orders.View};" +
          $"RequiredRole={Roles.Employee};" +
          $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "GetOrderDetails",
            Summary = "Fetch comprehensive details of a single shipment",
            Description = "### 🔍 Deep Order Inspection\n\n" +
                        "Retrieves a full diagnostic view of a specific shipment by its unique ID. This endpoint aggregates core shipment data with related entity names and itemized product lists.\n\n" +
                        "#### **⚙️ Data Composition:**\n" +
                        "| Section | Included Information |\n" +
                        "| :--- | :--- |\n" +
                        "| **Customer Info** | Name, primary/secondary phones, and specific delivery location. |\n" +
                        "| **Financials** | Calculated `ChargePrice`, `OrderPrice`, and the total `AmountReceived`. |\n" +
                        "| **Logistics Context** | Linked City, Branch, Charge Type, and Assigned Representative name. |\n" +
                        "| **Inventory** | A collection of `Products` including weights and quantities per item. |\n\n" +
                        "#### **🔒 Internal Retrieval Logic:**\n" +
                        "1. **ID Resolution**: Validates the ID against the primary `Orders` table. \n" +
                        "2. **Eager Loading**: Automatically resolves navigation properties using the **OrderSpecification** to avoid N+1 query issues. \n" +
                        "3. **Mapping**: Transforms the internal entity model into a flattened `OrderDTO` designed for frontend consumption.\n\n" +
                        "> **Authorization Note:** Access is strictly controlled via policies. Merchants can only retrieve orders they initiated, while staff accessibility depends on their group permissions.",
            Tags = new[] { "2. Operations & Order Lifecycle" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetOrderByIdSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(OrderNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _unitOfWork.OrderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"Order with ID {id} not found");
            
            return Ok(order);
        }

        #endregion

        #region Assign Order To Delivery

        [HttpPut]
        [Route("~/Orders/AssignToDelivery/{id}")]
        [Authorize(Policy =
        $"Permission={Orders.Edit};" +
        $"RequiredRole={Roles.Employee};" +
        $"AllowedRole={Roles.Admin}")]
        [SwaggerOperation(
            OperationId = "AssignShipmentToDriver",
            Summary = "Assign a shipment to a delivery representative",
            Description = "### 🚛 Logistics Assignment Workflow\n\n" +
                          "This endpoint transitions a shipment from the warehouse inventory to a specific delivery representative's custody. It enforces strict business rules to ensure valid state transitions.\n\n" +
                          "#### **⚙️ Strict Validation Rules:**\n" +
                          "| Rule | Logic | Action on Failure |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **Identity Verification** | Ensures the `ShippingRepresentativeId` exists in the system. | Returns **400 BadRequest** |\n" +
                          "| **Initial State Check** | Orders must be in `Pendding` state before assignment. | Returns **400 BadRequest** |\n" +
                          "| **New Order Block** | Orders in `New` state must be reviewed before assignment. | Returns **400 BadRequest** |\n" +
                          "| **Soft-Delete Guard** | Deleted orders cannot be processed for delivery. | Returns **400 BadRequest** |\n" +
                          "| **Duplicate Prevention** | Prevents re-assigning orders already with a representative. | Returns **400 BadRequest** |\n\n" +
                          "#### **🔒 Internal Execution Logic:**\n" +
                          "1. **Entity Retrieval**: Fetches the `Order` record by its unique ID. \n" +
                          "2. **State Analysis**: Checks current `OrderState` and `IsDeleted` flag. \n" +
                          "3. **Relationship Binding**: Links the `ShippigRepresentative` entity to the order. \n" +
                          "4. **State Transition**: Automatically promotes the status to `DeliveredToTheRepresentative`.\n\n" +
                          "> **Note:** This operation is reserved for Administrators and internal Employees with 'Edit' permissions.",
            Tags = new[] { "2. Operations & Order Lifecycle" }
        )]
        [SwaggerRequestExample(typeof(AssignOrderToDelivaryDTO), typeof(AssignOrderRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AssignOrderSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(AssignOrderBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignOrderToDelivery(int id, [FromBody] AssignOrderToDelivaryDTO statusDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _unitOfWork.OrderService.AssignOrderToDelivaryAsync(id, statusDTO);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        #endregion

        #region Get Orders By Status

        [HttpGet]
        [Route("~/Orders/GetByStatus/{status}")]
        [Authorize(Policy =
      $"Permission={Orders.View};" +
      $"RequiredRole={Roles.Employee};" +
      $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "GetOrdersByWorkflowStatus",
            Summary = "Filter shipments by their current lifecycle state",
            Description = "### 🚦 Status-Based Order Retrieval\n\n" +
                        "This endpoint allows for quick access to shipments grouped by a specific operational state (e.g., all `Delivered` orders or all `New` orders). It is highly optimized for dashboard counters and queue management.\n\n" +
                        "#### **⚙️ Workflow Logic:**\n" +
                        "| Step | Action | Implementation |\n" +
                        "| :--- | :--- | :--- |\n" +
                        "| **1. Param Parsing** | Status Mapping | Receives the status as a string and maps it to the internal `OrderState` Enum. |\n" +
                        "| **2. Spec Execution** | Target Selection | Invokes `OrderSpecification(status)` to generate a filtered SQL query. |\n" +
                        "| **3. Data Assembly** | Navigation Mapping | Fetches linked data (Cities, Merchants) and converts the entities into a list of `OrderDTO`. |\n\n" +
                        "#### **📋 Valid Status Examples:**\n" +
                        "`New`, `Pendding`, `DeliveredToTheRepresentative`, `Delivered`, `CanceledByCustomer`, etc.\n\n" +
                        "> **Note:** This endpoint returns a flat list of all matching records. For large datasets, it is recommended to use the `GetAll` endpoint with pagination parameters.",
            Tags = new[] { "2. Operations & Order Lifecycle" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetOrdersByStatusSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(InvalidStatusErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(List<OrderDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrdersByStatus(string status)
        {
            var orders = await _unitOfWork.OrderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        #endregion

        #region Get Products By Order Id

        [HttpGet]
        [Route("~/Orders/GetProductsByOrderId/{orderId}")]
        [Authorize(Policy =
      $"Permission={Orders.View};" +
      $"RequiredRole={Roles.Employee};" +
      $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "GetShipmentInventory",
            Summary = "Retrieve itemized inventory for a specific order",
            Description = "### 📦 Product Breakdown Workflow\n\n" +
                        "This endpoint provides an itemized list of all physical products associated with a specific shipment. It is essential for generating packing slips and calculating precise shipping weights.\n\n" +
                        "#### **⚙️ Internal Execution Flow:**\n" +
                        "| Step | Action | Logic Implementation |\n" +
                        "| :--- | :--- | :--- |\n" +
                        "| **1. Parent Validation** | Order Verification | First, queries the `Orders` repository to confirm the parent record exists. |\n" +
                        "| **2. Child Discovery** | Product Lookup | Executes a filtered query on the `Products` table using the `OrderId` foreign key. |\n" +
                        "| **3. Aggregation** | Projection | Maps internal product entities to `ProductDTO` objects including weight and quantity data. |\n\n" +
                        "#### **📋 Data Usage:**\n" +
                        "* **Weight Calculation**: Total shipment weight is derived from the sum of `Weight * Quantity` for all items in this list.\n" +
                        "* **Order Validation**: Ensuring the contents match the merchant's description before dispatch.\n\n" +
                        "> **Authorization Note:** Access is restricted to relevant stakeholders. Merchants can only view products for their own orders.",
            Tags = new[] { "2. Operations & Order Lifecycle" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetProductsByOrderSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(OrderProductsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(List<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductsByOrderId(int orderId)
        {
            var products = await _unitOfWork.OrderService.GetAllProductsByOrderIdAsync(orderId);
            return Ok(products);
        }

        #endregion

        #region CreateOrder

        [HttpPost]
        [Route("~/Orders/CreateOrder")]
        [Authorize(Policy =
            $"Permission={Orders.View};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant}")]
        [SwaggerOperation(
            OperationId = "PostCreateOrder",
            Summary = "Create a new shipment and calculate shipping fees",
            Description = "### 💳 Order Creation & Pricing Engine\n\n" +
                          "This endpoint validates shipment data and executes the core pricing algorithm to determine the final shipping cost based on multiple logistics variables.\n\n" +
                          "#### **⚖️ Pricing Algorithm Breakdown:**\n" +
                          "The final `ChargePrice` is calculated using the following priority logic:\n" +
                          "1. **Base City Price**: System checks for a `MerchantSpecialPrice` for the destination city. If not set, it defaults to the standard `City.ChargePrice`.\n" +
                          "2. **Rural Surcharge**: If `ShippingToVillage` is **true**, a fixed `VillagePrice` from the system standards is added.\n" +
                          "3. **Weight Penalty**: If the total weight exceeds the `StandardWeight` limit, an additional fee per KG (`KGprice`) is applied to the excess weight.\n" +
                          "4. **Pickup Fee**: If `OrderType` is 'PickupFromTheMerchant', a specialized pickup fee is added.\n" +
                          "5. **Service Level**: Adds the `ExtraPrice` defined by the selected `ChargeType` (e.g., Express Surcharge).\n\n" +
                          "#### **🔒 Internal Execution Steps:**\n" +
                          "| Step | Action | implementation Details |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **1. Validation** | Relationship Check | Verifies existence of City, Branch, ChargeType, and Merchant. |\n" +
                          "| **2. Inventory Sum** | Weight Analysis | Sums `Weight * Quantity` for all items to get `TotalWeight`. |\n" +
                          "| **3. Financials** | Cost Calculation | Executes the pricing engine and determines `AmountReceived` based on `PaymentType`. |\n" +
                          "| **4. Persistence** | Dual-Phase Save | Saves the `Order` first to generate an ID, then persists all linked `Products`. |\n\n" +
                          "> **Note:** Upon successful creation, the order is placed in the `New` state by default.",
            Tags = new[] { "2. Operations & Order Lifecycle" }
        )]
        [SwaggerRequestExample(typeof(CreateOrderDTO), typeof(CreateOrderRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CreateOrderSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CreateOrderBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.OrderService.CreateOrderAsync(orderDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        #endregion

        #region Update Order Status

        [HttpPut]
        [Route("~/Orders/UpdateOrder/{id}")]
        [Authorize(Policy =
              $"Permission={Orders.Edit};" +
              $"RequiredRole={Roles.Employee};" +
              $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "PutUpdateOrderStatus",
            Summary = "Update order state and operational notes",
            Description = "### 🔄 Shipment Lifecycle Update\n\n" +
                        "Allows authorized stakeholders to transition a shipment between different operational states. This endpoint is dynamic and validates the status against the system's predefined workflow constants.\n\n" +
                        "#### **⚙️ Internal Business Logic:**\n" +
                        "| Step | Action | Logic Implementation |\n" +
                        "| :--- | :--- | :--- |\n" +
                        "| **1. Order Lookup** | Retrieval | Fetches the `Order` entity using the provided ID. Returns **404** if not found. |\n" +
                        "| **2. Enum Parsing** | Dynamic Validation | Attempts to parse the `OrderState` string into the internal `OrderState` Enum. |\n" +
                        "| **3. Content Update** | Note Patching | If the `Notes` field is provided, it updates the existing order remarks. |\n" +
                        "| **4. Synchronization** | DB Update | Invokes `unitOfWork.Repository<Order>().Update` and persists changes. |\n\n" +
                        "#### **📋 Practical Usage Scenarios:**\n" +
                        "* **Merchant**: Updating a status to `CanceledByCustomer`.\n" +
                        "* **Representative**: Updating status to `Delivered` or `PostPoned`.\n" +
                        "* **Employee**: Correcting a status or adding administrative notes.\n\n" +
                        "> **Note:** Providing an invalid status string will result in a **400 BadRequest** error.",
            Tags = new[] { "2. Operations & Order Lifecycle" }
        )]
        [SwaggerRequestExample(typeof(UpdateOrderStatusDTO), typeof(UpdateOrderStatusRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UpdateOrderStatusSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(UpdateOrderStatusBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDTO statusDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _unitOfWork.OrderService.UpdateOrderStatusAsync(id, statusDTO);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
                
            return Ok(result.Message);
        }

        #endregion

        #region Delete Order
        [HttpDelete]
        [Route("~/Orders/DeleteOrder/{id}")]
        [Authorize(Policy =
            $"Permission={Orders.Delete};" +
            $"RequiredRole={Roles.Employee};" +
            $"AllowedRole={Roles.Admin},{Roles.Merchant},{Roles.ShippingRepresentative}")]
        [SwaggerOperation(
            OperationId = "DeleteShipment",
            Summary = "Perform a soft-delete on a specific shipment",
            Description = "### 🗑️ Soft-Delete Mechanism\n\n" +
                          "This endpoint implements a **Soft-Delete (Toggle)** pattern. Instead of removing the record from the database, it flips the `IsDeleted` flag. This ensures data auditability while removing the order from active operational views.\n\n" +
                          "#### **⚠️ Critical Business Constraints:**\n" +
                          "| Constraint | Logic | Action on Violation |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **State Guard** | Orders can **ONLY** be deleted if they are in the `New` state. | Returns **400 BadRequest** |\n" +
                          "| **Workflow Lock** | Once an order is `Pendding` or `Delivered`, deletion is prohibited to maintain financial integrity. | Returns **400 BadRequest** |\n" +
                          "| **ID Validation** | System verifies the order exists before attempting the toggle. | Returns **404 NotFound** |\n\n" +
                          "#### **🔒 Internal Execution Flow:**\n" +
                          "1. **Existence Check**: Queries the repository by `{id}`. \n" +
                          "2. **Logic Gate**: Validates that `OrderState == OrderState.New`. \n" +
                          "3. **Flag Toggle**: Executes `order.IsDeleted = !order.IsDeleted` (Allowing for restoration if called again). \n" +
                          "4. **Persistence**: Commits the change via `unitOfWork.Save()`. \n\n" +
                          "> **Note:** This operation requires 'Delete' permissions and is restricted based on the user's role and ownership of the order.",
            Tags = new[] { "2. Operations & Order Lifecycle" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteOrderSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DeleteOrderBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(DeleteOrderNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _unitOfWork.OrderService.DeleteOrderAsync(id);
            
            if (!result.IsSuccess)
                return NotFound(result.Message);
                
            return Ok(result.Message);
        }

        #endregion

        #region AddRange Orders

        //[HttpPost]
        //[Route("~/Orders/CreateManyOrder")]
        //[Authorize(Policy =
        //    $"Permission={Orders.View};" +
        //    $"RequiredRole={Roles.Employee};" +
        //    $"AllowedRole={Roles.Admin},{Roles.Merchant}")]
        //public async Task<IActionResult> CreateManyOrder([FromBody] List<CreateOrderDTO> orderDTOs)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var result = await _unitOfWork.OrderService.CreateOrdersAsync(orderDTOs);

        //    if (!result.IsSuccess)
        //        return BadRequest(result.Message);

        //    return Ok(result.Message);
        //}

        #endregion
    }
} 