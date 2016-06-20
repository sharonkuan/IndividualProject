namespace SupportApp.Controllers {

    export class EventDeleteDialogController {

        public eventToDelete;
        public validationErrors;

        constructor(private eventIdFrom,
            private eventServices: SupportApp.Services.EventServices,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {

            this.getEvent();
        }

        getEvent() {

            this.eventToDelete = this.eventServices.getUserEventDetails(this.eventIdFrom);
            debugger;
        }

        deleteEvent() {
            debugger;
            this.eventServices.deleteEvent(this.eventToDelete.id).then(() => {
                this.$uibModalInstance.close();
                //this.eventServices.getAllEvents();
            }).catch((error) => {
                let validationErrors = [];
                for (let prop in error.data) {
                    let propErrors = error.data[prop];
                    validationErrors = validationErrors.concat(propErrors);
                }
                this.validationErrors = validationErrors;
            })
        }

        close() {
            this.$uibModalInstance.close();
        }
    }
}