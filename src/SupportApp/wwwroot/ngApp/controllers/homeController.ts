namespace SupportApp.Controllers {

    export class HomeController {
        public message = 'Hello from the home page!';
        public events;
        private selectedEventLocation;
        public eventLocations;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.getActiveEvents();
            debugger;
        }

        getActiveEvents() {
            debugger;
            this.eventServices.getActiveEvents().then((data) => {
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

        public slides = [
            { image: "https://pbs.twimg.com/media/CZt7g5_WwAAY9F6.jpg", text: "elderlyPeople", id: 0 },
            { image: "http://alone.ie/wp-content/uploads/2015/11/th.jpg", text: "elderlyPeople1", id: 1 },
            { image: "http://www.west-info.eu/files/ImageHandler.ashx_.jpg", text: "elderlyPeople2", id: 2 },
            { image: "https://futureyouhealth.com/wp/wp-content/uploads/2015/06/O65-Banner-600x200.jpg", text: "elderlyPeople3", id: 3 }];
        public myInterval = 3000;
        public noWrapSlides = false;
        public active = 0;
    }
}