namespace SupportApp.Controllers {

    export class MyHistoryEventDetailController {

        public event;
        public eventComment; //passed from view 
        private eventId;
        public canEdit;
        public validationErrors;


        constructor(private eventServices: SupportApp.Services.EventServices,
            $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService) {

            this.eventId = $stateParams["id"];
            this.getEvent();
            this.initialize();
        }

        getEvent() {
            
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

        //worked
        saveComment() {
            this.eventServices.saveEventComment(this.eventId, this.eventComment).then((data) => {
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

        //this accepts the value set by the ng-click button value to pass to API controller 
        voteEvent(voteType) {
            this.eventServices.voteEvent(this.eventId, voteType).then((data) => {
                
                this.event = data;
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
            
            this.$state.go("myHistoryEvents");
        }

        initialize() {
            this.eventComment = {};
            this.eventComment.message = "";
        }

        clearCommentForm() {
            let element: any = <HTMLTextAreaElement>document.getElementById("commentForm");
            element.reset();
            this.eventComment = "";
            this.validationErrors = null;
        }
    }
}