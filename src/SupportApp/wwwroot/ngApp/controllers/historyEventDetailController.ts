namespace SupportApp.Controllers {

    export class HistoryEventDetailController {

        public event;
        private eventId;
        public canEdit;
        public validationErrors;

        constructor(private eventServices: SupportApp.Services.EventServices,
            $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService) {

            this.eventId = $stateParams["id"];
            this.getHistoryEvent();
        }

        getHistoryEvent() {
            //debugger;
            this.eventServices.getEventDetails(this.eventId).then((data) => {
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
    }
} 