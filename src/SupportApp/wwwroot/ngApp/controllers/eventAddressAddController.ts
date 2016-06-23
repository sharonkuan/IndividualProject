namespace SupportApp.Controllers {

    export class EventAddressAddController {

        public event;  //from view
        public eventLocation;
        public canEdit; //from server
        public validationErrors;
        private eventId;  //local extract

        constructor(private eventServices: SupportApp.Services.EventServices,
            $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService) {
            this.eventId = $stateParams["id"];
            this.initialize();
        }

        getMyEvent() {
            
            this.eventServices.getUserEventDetails(this.eventId).then((data) => {
                this.event = data.event;
                this.canEdit = data.canEdit;
                console.log(data);
            }).catch((err) => {
                let validationErrors = [];
                for (let prop in err.data) {
                    let propErrors = err.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            });
        }

        saveAddedEventLocation() {
            this.eventServices.saveEventComment(this.eventId, this.eventLocation).then((data) => {
                //console.log("data: " + data);
                this.event = data;
                
                this.clearCommentForm();
            }).catch((err) => {
                let validationErrors = [];
                for (let prop in err.data) {
                    let propErrors = err.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            });
        }

        cancel() {
            this.$state.go("adminEvents");
        }

        initialize() {
            this.eventLocation = {};
            this.eventLocation.message = "";
        }

        clearCommentForm() {
            let element: any = <HTMLTextAreaElement>document.getElementById("commentForm");
            element.reset();
            this.eventLocation = "";
            this.validationErrors = null;
        }
    }
}