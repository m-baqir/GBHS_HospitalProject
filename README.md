# GBHS_HospitalProject for HTTP-5204
---
Team Members:
- Mohammad Baqir - Feature: Location and Service Directory. Entities: (Location, Service)
- Ambaram Srivastava
- Suong Tran - Feature : Appointment booking. Enitities: (Department, Specialist)
- Natalya Kolesnikova - Feature: Feedback Form. Entities: (Feedback)
- Thai Vo - Feature : Appointment booking. Enitities: (Patient, Booking)
---
### Location&Services - *Mohammad Baqir*
These two entities are connected together because there are many services offered at each location and many locations can have different services. As such, these two entities have a Many-to-Many relationship. They are connected via a bridge table and my code has the ability to create and destroy this link between these two entities. 
- List: The list method for both entities lists all locations or services on their respective pages.
- Details: The details page for a particular service lists locations that **DO** offer that particular service as well as locations that **DO NOT** offer that service. Similarly, the details page of each location lists services that **ARE** offered there as well as services that are **NOT** offered at that particular location.
The remaing pages are designed for admin users and allow for modifications to the website.
- ADD: Both new services and locations can be added using form elements on the NEW page, which in turn creates a new entry in the database.
- UPDATE: Similarly, the EDIT page allows for the admin to update details of a location or service using form fields and then the change gets reflected in the database accordingly.
- DELETE: Finally, the delete confirm page gives the admin a warning page before proceeding with the deletion of a particular entity. This applies to both services and locations. 
