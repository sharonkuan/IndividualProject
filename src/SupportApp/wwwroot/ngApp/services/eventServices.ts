namespace SupportApp.Services {

    export class EventServices {

        private eventsResources;
        private eventCommentResources;
        private eventLocationResources;

        constructor($resource: angular.resource.IResourceService) {

            this.eventsResources = $resource("/api/event/:id", null, {
                vote: {
                    method: "PUT",
                    url: "/api/event/:id"
                }
            });
            this.eventCommentResources = $resource("/api/eventComment/:id");
            this.eventLocationResources = $resource("/api/eventLocation/:id");
        }

        getAllEvents() {

            return this.eventsResources.query();
        }

        getSingleEvent(eventId) {

            return this.eventsResources.get({ id: eventId });
        }

        voteEvent(eventId, voteValue) {
            //alert(questionId);
            return this.eventsResources.vote({ id: eventId }, voteValue).$promise;
        }

        saveEvent(event) {

            return this.eventsResources.save(event).$promise;
        }

        //save a single event comment
        saveEventLocation(eventId, locationToSave) {
            return this.eventLocationResources.save({ id: eventId }, locationToSave).$promise;
        }

        //save a single event comment
        saveEventComment(eventId, commentToSave) {
            debugger;
            return this.eventCommentResources.save({ id: eventId }, commentToSave).$promise;
        }

        deleteEvent(eventId) {
            //debugger;
            //this.eventCommentResources.remove({ id: eventId }).$promise;
            //this.eventLocationResources.remove({ id: eventId }).$promise;
            return this.eventsResources.remove({ id: eventId }).$promise;
        }
    }

    angular.module("SupportApp").service("eventServices", EventServices);
}