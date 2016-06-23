namespace SupportApp.Controllers {

    export class HomeController {
        public events;
        private selectedEventLocation;
        public eventLocations;
        public validationErrors;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private $uibModal: ng.ui.bootstrap.IModalService) {

            this.getActiveEvents();
        }

        getActiveEvents() {
            //
            this.eventServices.getActiveEvents().then((data) => {
                this.events = data.events;
                this.selectedEventLocation = "All";
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
            //
            console.log(this.selectedEventLocation);
            this.eventServices.searchByCity(this.selectedEventLocation).then((data) => {
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

        public slides = [
            { image: "https://pbs.twimg.com/media/CZt7g5_WwAAY9F6.jpg", text: "elderlyPeople", id: 0 },
            { image: "http://alone.ie/wp-content/uploads/2015/11/th.jpg", text: "elderlyPeople1", id: 1 },
            { image: "http://www.west-info.eu/files/ImageHandler.ashx_.jpg", text: "elderlyPeople2", id: 2 },
            { image: "https://futureyouhealth.com/wp/wp-content/uploads/2015/06/O65-Banner-600x200.jpg", text: "elderlyPeople3", id: 3 },
            { image: "http://lntsf.com/uploads/3/4/3/6/34369855/6485525_orig.jpg", text: "elderlyPeople4", id: 4 },
            { image: "http://simmaronresearch.com/wp-content/uploads/2014/04/Maturing-Person-477315.jpg", text: "elderlyPeople5", id: 5 }];
        public myInterval = 3000;
        public noWrapSlides = false;
        public active = 0;
    }
}