namespace SupportApp.Controllers {

    export class EventsDetailController {

        public event;
        public eventComment; //passed from view 
        private eventId;
        public validationErrors;


        constructor(private eventServices: SupportApp.Services.EventServices,
            $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService) {

            this.eventId = $stateParams["id"];
            this.getEvent();
            this.initialize();        }

        getEvent() {
            this.eventServices.getSingleEvent(this.eventId).then((data) => {
                this.event = data;
            });
        }

        saveComment() {
            this.eventServices.saveEventComment(this.eventId, this.eventComment).then(() => {
                let element: any = <HTMLTextAreaElement>document.getElementById("commentForm");
                element.reset();
                this.eventComment = "";
                this.validationErrors = null;
                this.getEvent();  //include the latest comments added
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
                //this.getEvent();
                debugger;
                this.event = data;
                
            });
        }

        cancel() {
            this.$state.go("events");
        }

        initialize() {
            this.eventComment = {};
            this.eventComment.message = "";
        }
    }
    angular.module("SupportApp").controller("eventsDetailController", function ($rootScope, $scope, $filter) {
        var filterdatetime = $filter('datetmUTC')($scope.date);
    });
} 