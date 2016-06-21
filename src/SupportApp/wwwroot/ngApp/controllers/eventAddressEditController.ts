namespace SupportApp.Controllers {

    export class EventAddressEditController {

        public eventAddressToEdit;
        private eventId;
        public validationErrors;


        constructor(private eventServices: SupportApp.Services.EventServices,
            $stateParams: angular.ui.IStateParamsService,
            private $state: angular.ui.IStateService) {

            this.eventId = $stateParams["id"];
            this.getEvent();
        }

        getEvent() {

            this.eventServices.getUserEventDetails(this.eventId).then((data) => {
                this.eventAddressToEdit = data.event;
                this.eventAddressToEdit.eventStartDate = new Date(this.eventAddressToEdit.eventStartDate);
                this.eventAddressToEdit.isPrivate = getSelection();
                this.eventAddressToEdit.eventEndDate = new Date(this.eventAddressToEdit.eventEndDate);
            });
        }
        saveEvent() {
            this.eventServices.saveEvent(this.eventAddressToEdit).then((data) => {
                console.log("Saved data: " + data);
                debugger;
                this.$state.go("myEvents");
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
            this.$state.go("eventEdit");
        }
    }
}