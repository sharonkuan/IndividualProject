﻿namespace SupportApp.Controllers {

    export class AdminHistoryEventDetailController {
        public event;
        public eventComment; //passed from view 
        private eventId;
        public canEdit;
        public validationErrors;
        public volunteers;


        constructor(private eventServices: SupportApp.Services.EventServices,
            $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService) {

            this.eventId = $stateParams["id"];
            this.getEvent();
            this.getVolunteerInfo();
            this.initialize();
        }

        getEvent() {
            
            this.eventServices.getUserEventDetails(this.eventId).then((data) => {
                this.event = data.event;
                this.canEdit = data.canEdit;
                console.log(data);
            }).catch(() => {
                console.log("failed");
            });
        }

        getVolunteerInfo() {
            debugger;
            this.eventServices.getVolunteerInfo(this.eventId).then((data) => {
                this.volunteers = data;
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
            
            this.$state.go("adminHistoryEvents");
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