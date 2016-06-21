namespace SupportApp.Controllers {

    export class EventDeleteDialogController {

        public eventToDelete;
        public validationErrors;
        public canEdit;

        constructor(private eventIdFrom,
            private eventServices: SupportApp.Services.EventServices,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {

            this.getEvent();
        }

        getEvent() {
            this.eventToDelete = this.eventServices.getUserEventDetails(this.eventIdFrom).then((data) => {
                debugger;
                this.eventToDelete = data.event;
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

        deleteEvent() {
            debugger;
            this.eventServices.deleteEvent(this.eventToDelete.id).then((data) => {
                debugger;
                this.eventToDelete = data.event;
                this.close();
                location.reload(true);
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