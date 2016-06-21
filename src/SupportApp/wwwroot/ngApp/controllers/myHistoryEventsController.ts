namespace SupportApp.Controllers {

    export class MyHistoryEventsController {

        public events;
        private selectedEventLocation;
        public eventLocations;
        public canEdit;
        public hasClaim;
        public validationErrors;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.getUserHistoryEvents();
            debugger;
        }

        getUserHistoryEvents() {
            debugger;
            this.eventServices.getUserHistoryEvents().then((data) => {
                console.log(data);
                debugger;
                this.events = data.events;
                this.selectedEventLocation = "All";
                this.canEdit = data.canEdit;
                this.hasClaim = data.hasClaim;
                this.eventLocations = this.extractingEventNestedArray();
            });
        }

        searchEventsByCity() {
            debugger;
            console.log(this.selectedEventLocation);
            this.eventServices.searchMyActiveHistoryEvents(this.selectedEventLocation).then((data) => {
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
            //debugger;
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