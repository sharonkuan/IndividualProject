namespace SupportApp.Controllers {

    export class EventsController {

        public events;
        private selectedEventLocation;
        public eventLocations;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.getAllEvents();
            debugger;
            
        }

        getAllEvents() {

            this.eventServices.getAllEvents().then((data) => {
                this.events = data;
                this.eventLocations = this.extractingEventNestedArray();
            });
        }

        showDeleteDialog(eventId) {
            //debugger;
            this.$uibModal.open({
                templateUrl: '/ngApp/views/eventDeleteDialog.html',
                controller: SupportApp.Controllers.EventDeleteDialogController,
                controllerAs: 'controller',
                resolve: {
                    eventIdFrom: () => eventId
                },
                size: 'sm'
            });
        }

        //worked, this one extracts only the locations from all events
        extractingEventNestedArray() {

            var objArray = [];
            for (let singleEvent of this.events) {
                for (let location of singleEvent.locations) {
                    objArray.push(location);
                }
            }
            return objArray;
        }
    }
}