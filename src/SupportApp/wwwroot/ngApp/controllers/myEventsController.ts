namespace SupportApp.Controllers {

    export class MyEventsController {

        public events;
        private selectedEventLocation;
        public eventLocations;
        public validationErrors
        public canEdit;
        public hasClaim;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.getMyEvents();
            debugger;
        }

        getMyEvents() {
            debugger;
            this.eventServices.getUserEvents().then((data) => {
                console.log(data);
                debugger;
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
            debugger;
            console.log(this.selectedEventLocation);
            this.eventServices.searchMyActiveEvents(this.selectedEventLocation).then((data) => {
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