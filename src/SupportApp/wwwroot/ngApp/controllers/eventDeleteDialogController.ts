namespace SupportApp.Controllers {

    export class EventDeleteDialogController {

        public eventToDelete;
        public validationErrors;

        constructor(private eventServices: SupportApp.Services.EventServices,
            private eventIdFrom,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {

            this.getEvent();
        }

        getEvent() {

            this.eventToDelete = this.eventServices.getSingleEvent(this.eventIdFrom);           
        }

        deleteEvent() {
            this.eventServices.deleteEvent(this.eventToDelete).then(() => {
                this.$uibModalInstance.close();
            }).catch((error) => {
                for (let prop in error.data) {
                    let propError = error.data(prop);
                    this.validationErrors = this.validationErrors.concat(propError);
                }

                })
        }

        close() {
            this.$uibModalInstance.close();
        }
    }
}