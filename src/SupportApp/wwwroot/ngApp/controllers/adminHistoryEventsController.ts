namespace SupportApp.Controllers {

    export class AdminHistoryEventsController {

        public events;
        private selectedEventLocation;
        public eventLocations;
        public validationErrors;
        public canEdit;
        public hasClaim;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.getAllEvents();
            
        }

        getAllEvents() {
            
            //put all events in viewmodel came back as object
            this.eventServices.getAllHistoryEvents().then((data) => {
                this.events = data.events;
                this.selectedEventLocation = "All";
                this.canEdit = data.canEdit;
                this.hasClaim = data.hasClaim;
                this.eventLocations = this.extractingEventNestedArray();
            }).catch((err) => {
                let validationErrors = [];
                for (let prop in err.data) {
                    let propErrors = err.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            });
        }

        searchEventsByCity() {
            
            console.log(this.selectedEventLocation);
            this.eventServices.searchAllHistoryEvents(this.selectedEventLocation).then((data) => {
                this.events = data;
            }).catch((err) => {
                let validationErrors = [];
                for (let prop in err.data) {
                    let propErrors = err.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            });
        }

        showDeleteDialog(eventId) {
            //
            this.$uibModal.open({
                templateUrl: '/ngApp/views/eventDeleteDialog.html',
                controller: SupportApp.Controllers.EventDeleteDialogController,
                controllerAs: 'controller',
                resolve: {
                    eventIdFrom: () => eventId  //this eventId is passed from the form
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