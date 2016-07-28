namespace SupportApp.Controllers {

    export class ActiveEventDetailController {

        public event;  //from view
        public eventComment;
        public canEdit; //from server
        public validationErrors;
        private eventId;  //local extract
        public confirmVolunteer;


        constructor(private eventServices: SupportApp.Services.EventServices,
            $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService, private moment) {

            this.eventId = $stateParams["id"];
            this.getEvent();
            this.initialize();
        }

        getEvent() {
            this.eventServices.getEventDetails(this.eventId).then((data) => {
                let self = this;
                self.event = data.event;
                for (let comment of self.event.comments) {
                    debugger;
                    console.log(comment.applicationUserId);
                    comment.dateCreated = this.moment(comment.dateCreated).fromNow();
                }
                this.canEdit = data.canEdit;
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

        signUpToVolunteer() {
            debugger;
            this.eventServices.signUpToVolunteer(this.eventId, false).then(() => {
                this.confirmVolunteer = "Thank you for volunteering this event!";
            });
        }

        cancel() {
            this.$state.go("home");
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