namespace SupportApp.Controllers {

    export class EventEditController {

        public eventToEdit;
        private eventId;
        public validationErrors;

        constructor(private eventServices: SupportApp.Services.EventServices,
            $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService) {

            this.eventId = $stateParams["id"];
            this.getEvent();
        }

        getEvent() {

            this.eventToEdit = this.eventServices.getSingleEvent(this.eventId);
        }

        editEvent() {
            this.eventToEdit = this.eventServices.saveEvent(this.eventToEdit).then(() => {
                this.$state.go("events");
            }).catch((error) => {
                let validationErrors = [];
                for (let prop in error.data) {
                    let propErrors = error.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            })
        }

        cancel() {
            this.$state.go("events");
        }
    }
}