using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
namespace ShippingManagementSystem.Web.Swagger.Filter
{
    public class TagDescriptionsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var tagsToUpdate = new List<OpenApiTag>
                {
                    new OpenApiTag
                    {
                        Name = "1. Security & Identity",
                        Description = "### 🔐 Identity Governance & Access Management\n\n" +
                          "This section handles the complete lifecycle of user identity, secure authentication, and granular authorization using **ASP.NET Core Identity** and **JWT Bearer** tokens.\n\n" +
                          "#### **📂 Core Modules:**\n" +
                          "* **Authentication**: Secure multi-step validation for local and administrative accounts.\n" +
                          "* **Account Recovery**: Fully automated password reset workflow integrated with **SMTP (MailKit)** services.\n" +
                          "* **Identity Profile**: Management of personal information and secure credential rotation.\n" +
                          "* **RBAC & Permissions**: A sophisticated role-based access control system that maps user groups to functional modules.\n\n" +
                          "#### **🛡️ Security Compliance:**\n" +
                          "| Security Layer | Implementation Strategy | Reference |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **Auth Strategy** | Stateless JWT (Json Web Token) | `Bearer Scheme` |\n" +
                          "| **Password Security** | PBKDF2 with SHA256 Hashing | `Identity Default` |\n" +
                          "| **Data Integrity** | Cryptographic Security Stamps | `Session Invalidation` |\n" +
                          "| **Email Security** | SSL/TLS Encrypted Transmission | `MailKit Service` |\n\n" +
                          "#### **🚦 Authorization Flow:**\n" +
                          "1. **Login**: User exchanges credentials for a JWT token.\n" +
                          "2. **Claims**: The token contains `Role` and `Permission` claims mapped from the database.\n" +
                          "3. **Validation**: Protected endpoints enforce policies based on these claims.\n\n" +
                          "> **💡 Integration Tip:** All requests to endpoints with the 🔒 icon must include the header: `Authorization: Bearer {your_jwt_token}`."
                    },
                    new OpenApiTag
                    {
                        Name = "2. Operations & Order Lifecycle",
                        Description = "### 📦 Logistics & Dispatch Intelligence Engine\n\n" +
                          "The core engine governing the journey of a shipment from merchant creation to final customer delivery. This engine handles high-precision pricing, state-machine transitions, and inventory tracking.\n\n" +
                          "#### **📂 Core Logistics Pillars:**\n" +
                          "* **Dynamic Pricing Framework**: A multi-variable algorithm calculating fees based on `StandardWeight`, `CitySurcharges`, and `SpecialMerchantRates`.\n" +
                          "* **State Machine Logic**: Enforces strict workflow rules, ensuring orders only transition between valid logical states (e.g., `New` → `Pendding` → `Delivered`).\n" +
                          "* **Fleet Assignment**: Manages the hand-over process between central branches and shipping representatives.\n" +
                          "* **Soft-Delete Auditability**: Protects financial integrity by preventing physical deletion of active shipments.\n\n" +
                          "#### **⚙️ The Pricing Equation:**\n" +
                          "The system automatically resolves cost using the following hierarchy:\n" +
                          "1. **Merchant/City Overrides**: Checks for customized pricing agreements first.\n" +
                          "2. **Standard Logistics Fees**: Applies base city rates and pickup surcharges.\n" +
                          "3. **Physical Constraints**: Calculates extra weight penalties per KG beyond standard limits.\n\n" +
                          "#### **🚦 Workflow Integrity & Access Control:**\n" +
                          "| Operation | Permission Required | State Restriction | Transition To |\n" +
                          "| :--- | :--- | :--- | :--- |\n" +
                          "| **Creation** | `Orders.Create` | N/A | `New` |\n" +
                          "| **Assignment** | `Orders.Edit` | Must be `Pendding` | `Representative` |\n" +
                          "| **Deletion** | `Orders.Delete` | Only allowed if `New` | `IsDeleted = true` |\n\n" +
                          "> **💡 Note:** Financial metrics like `AmountReceived` are automatically adjusted based on the `PaymentType` (e.g., COD vs Exchange) and the `OrderState` results."
                    },
                    new OpenApiTag
                    {
                        Name = "3. Business Partners (Merchants & Reps)",
                        Description = "### 🤝 Stakeholder & Logistics Intelligence\n\n" +
                          "This module is the **Command Center** for managing the external human and business capital of the shipping ecosystem. It seamlessly bridges core user identities with complex logistics and financial configurations.\n\n" +
                          "--- \n\n" +
                          "#### **📂 Data Architecture & Roles**\n" +
                          "The system employs an **Extended Identity Model** (Polymorphism). Every partner starts as a base `ApplicationUser` and is then specialized into one of two strategic roles:\n\n" +
                          "| Entity | Core Responsibility | Unique Logistics Logic |\n" +
                          "| :--- | :--- | :--- |\n" +
                          "| **Merchants** | Supply Chain Origin | **Custom Pricing Engine**: Overrides standard city rates with merchant-specific agreements. |\n" +
                          "| **Shipping Reps** | Last-Mile Delivery | **Territorial Coverage**: Dynamic assignment to specific Governorates for localized dispatching. |\n\n" +
                          "#### **⚙️ Operational Guardrails**\n" +
                          "* **Atomic Transactions**: Onboarding a partner is an 'All-or-Nothing' process. The system synchronizes Identity, Roles, Branches, and Pricing in a single protected transaction.\n" +
                          "* **Soft-Delete Lifecycle**: We never erase history. Partners are 'Toggled' (Activated/Deactivated) to maintain the integrity of financial audits and delivery manifests.\n\n" +
                          "#### **💰 Financial Framework**\n" +
                          "Each partner type has a distinct financial footprint:\n" +
                          "1. **Merchants**: Focused on `RejectedOrderPercentage` and `SpecialPickUp` fees.\n" +
                          "2. **Shipping Reps**: Driven by `DiscountType` (Fixed/Percentage) and `CompanyPercentage` commissions.\n\n" +
                          "> **💡 Integration Tip:** Use the `IsActive` filter in query parameters to toggle between operational and archived partners in your dashboard views."
                    },
                    new OpenApiTag
                    {
                        Name = "4. System Configuration",
                        Description = "### 🏗️ Logistics Infrastructure & System Constants\n\n" +
                                      "This section manages the **Master Data** and foundational settings that drive the entire shipping engine. It defines the geographical, operational, and financial constraints of the platform.\n\n" +
                                      "#### **🌍 1. Geographical Governance (Locations)**\n" +
                                      "Managed via `Governorates` and `Cities`. This hierarchy establishes the primary zones for shipping and determines the **Default Base Pricing** for deliveries and pick-ups.\n\n" +
                                      "#### **🏢 2. Operational Hubs (Branches)**\n" +
                                      "Defines physical sorting centers and distribution points. Each **Branch** is a logistics hub tied to a specific city, ensuring localized order management.\n\n" +
                                      "#### **⚖️ 3. Financial Base Standards (Pricing Constants)**\n" +
                                      "Defines the **Core Mathematical Constants** of the pricing engine, including:\n" +
                                      "* **Weight Thresholds**: The base weight limit for standard shipping fees.\n" +
                                      "* **Overweight Rates**: Cost per extra KG beyond the standard limit.\n" +
                                      "* **Rural Surcharges**: Fixed additional fees for village/remote area deliveries.\n\n" +
                                      "#### **⚡ 4. Service Levels (Charge Types)**\n" +
                                      "Configures delivery speed tiers (e.g., *Normal*, *Express*). These define the **Financial Multipliers** and **Time SLAs** (Number of Days) for every order.\n\n" +
                                      "#### **🚫 5. Quality Control (Rejected Reasons)**\n" +
                                      "A standardized library of failure justifications providing a **Legal & Operational Audit Trail** for unfulfilled or returned shipments.\n\n" +
                                      "--- \n" +
                                      "> **⚠️ Critical Warning:** Changes in this section have a global impact on order pricing, delivery estimates, and historical reporting accuracy. Access should be restricted to Top-Level Admins."
                    },
                    new OpenApiTag
                    {
                        Name = "5. Administrative Control & Analytics",
                        Description = "### 🏛️ Executive Governance, Staffing & Financial Intelligence\n\n" +
                                      "The **Central Command & Control** module of the Shipping Management System. This section empowers high-level administrators to oversee the entire logistics lifecycle, manage human resources, and calibrate the system's financial engine.\n\n" +
                                      "--- \n\n" +
                                      "#### **📊 1. Business Intelligence (BI) & Analytics**\n" +
                                      "Provides an executive-level **Dashboard** with real-time data aggregation. It monitors:\n" +
                                      "* **Performance KPIs**: Delivery efficiency rates and monthly growth trends.\n" +
                                      "* **Financial Auditing**: Revenue tracking, average shipping costs, and weight distributions.\n" +
                                      "* **Operational Health**: Order state distribution and payment type analytics for strategic planning.\n\n" +
                                      "#### **👥 2. Workforce & Access Management (RBAC)**\n" +
                                      "A comprehensive suite for managing the company’s internal hierarchy and security posture:\n" +
                                      "* **Employee Directory**: Full CRUD operations for administrative staff, linked to physical branches.\n" +
                                      "* **Permission Groups**: Granular **Role-Based Access Control (RBAC)**. Define 'Who can do What' by mapping security groups to specific system modules (Orders, Reports, etc.).\n" +
                                      "* **Claim Synchronization**: Dynamic injection of security claims into user tokens based on their assigned group.\n\n" +
                                      "#### **⚖️ 3. Global Shipping Standards & Pricing**\n" +
                                      "The 'Pricing Constitution' of the application. It regulates the mathematical logic for all cost calculations:\n" +
                                      "* **Weight Thresholds**: Setting the base 'Standard Weight' for the entire fleet.\n" +
                                      "* **Revenue Protection**: Defining 'Price per Extra KG' and 'Remote Area (Village) Surcharges'.\n" +
                                      "* **Configurable Guardrails**: Immediate global updates to pricing logic without code changes.\n\n" +
                                      "--- \n" +
                                      "#### **🔐 Security Protocol:**\n" +
                                      "Endpoints in this section require **High-Privilege Bearer Tokens**. Unauthorized access attempts are logged. Changes to **Groups** or **Standards** are atomic and affect all associated users and calculations in real-time."
                    }
                };

            foreach (var tag in tagsToUpdate)
            {
                var existingTag = swaggerDoc.Tags.FirstOrDefault(t => t.Name == tag.Name);

                if (existingTag != null)
                    existingTag.Description = tag.Description;
                else
                    swaggerDoc.Tags.Add(tag);
            }
        }
    }

}
