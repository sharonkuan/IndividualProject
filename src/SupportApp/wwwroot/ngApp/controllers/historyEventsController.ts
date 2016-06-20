namespace SupportApp.Controllers {

    export class HistoryEventsController {

        public events;
        private selectedEventLocation;
        public eventLocations;
        public validationErrors;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.getHistoryEvents();
            //debugger;
        }

        getHistoryEvents() {
            //debugger;
            this.eventServices.getHistoryEvents().then((data) => {
                this.events = data.events;
                this.selectedEventLocation = "All";
                this.eventLocations = this.extractingEventNestedArray();
            });
        }

        //searchActiveHistoryEventsByCity
        searchEventsByCity() {
            debugger;
            console.log(this.selectedEventLocation);
            this.eventServices.searchActiveHistoryEventsByCity(this.selectedEventLocation).then((data) => {
                this.events = data;
                //debugger;
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