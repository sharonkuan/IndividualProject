namespace SupportApp.Controllers {

    export class HistoryEventsController {

        public message = 'Hello from the home page!';
        public events;
        private selectedEventLocation;
        public eventLocations;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.getHistoryEvents();
            //debugger;
        }

        getHistoryEvents() {
            //debugger;
            this.eventServices.getHistoryEvents().then((data) => {
                this.events = data.events;
                this.eventLocations = this.extractingEventNestedArray();
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