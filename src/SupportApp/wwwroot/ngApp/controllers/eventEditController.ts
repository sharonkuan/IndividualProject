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

            this.eventServices.getUserEventDetails(this.eventId).then((data) => {
                this.eventToEdit = data.event;
                this.eventToEdit.eventStartDate = new Date(this.eventToEdit.eventStartDate);
                this.eventToEdit.isPrivate = getSelection();
                this.eventToEdit.eventEndDate = new Date(this.eventToEdit.eventEndDate);
            });
        }

        saveEvent() {
            this.eventServices.saveEvent(this.eventToEdit).then((data) => {
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
            this.$state.go("events");
        }

        checkPrivate() {
            debugger;
            if (this.eventToEdit.isPrivate == true) {
                let elem = <HTMLInputElement>document.getElementById("isPrivate");
                elem.checked = true;
            }
            else {
                let elem = <HTMLInputElement>document.getElementById("isNotPrivate");
                elem.checked = true;
            }
        }

        checkVolunteerRequired() {
            debugger;
            if (this.eventToEdit.isVolunteerRequired == "Yes") {
                let elem = <HTMLInputElement>document.getElementById("isVolunteerRequired");
                elem.checked = true;
            }
            else if (this.eventToEdit.isVolunteerRequired == "No")
            {
                let elem = <HTMLInputElement>document.getElementById("isVolunteerNotRequired");
                elem.checked = true;
            }
            else if (this.eventToEdit.isVolunteerRequired == "Maybe")
            {
                let elem = <HTMLInputElement>document.getElementById("isVolunteerMaybeRequired");
                elem.checked = true;
            }
            
        }
    }
}