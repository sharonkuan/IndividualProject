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
                },
                search: {
                    method: 'GET',
                    url: '/api/event/search/:city',
                    isArray: true
                },
                searchActiveHistoryEvents: {
                    method: 'GET',
                    url: '/api/event/searchactivehistoryevents/:city',
                    isArray: true
                },
                searchMyActiveEvents: {
                    method: 'GET',
                    url: '/api/event/searchmyactiveevents/:city',
                    isArray: true
                },
                searchMyActiveHistoryEvents: {
                    method: 'GET',
                    url: '/api/event/searchmyactivehistoryevents/:city',
                    isArray: true
                },
                searchAllEvents: {
                    method: 'GET',
                    url: '/api/event/searchallevents/:city',
                    isArray: true
                },
                searchAllHistoryEvents: {
                    method: 'GET',
                    url: '/api/event/searchallhistoryevents/:city',
                    isArray: true
                },
                getActiveEvents: {
                    method: 'GET',
                    url: '/api/event/getactiveevents',
                    isArray: false
                },
                getHistoryEvents: {
                    method: 'GET',
                    url: '/api/event/gethistoryevents',
                    isArray: false
                },
                getUserEvents: {
                    method: 'GET',
                    url: '/api/event/getuserevents',
                    isArray: false
                },
                getUserHistoryEvents: {
                    method: 'GET',
                    url: '/api/event/getuserhistoryevents',
                    isArray: false
                },
                getAllEvents: {
                    method: 'GET',
                    url: '/api/event/getallevents',
                    isArray: false
                },
                getAllHistoryEvents: {
                    method: 'GET',
                    url: '/api/event/getallhistoryevents',
                    isArray: false
                },
                //event detail
                getEventDetails: {
                    method: 'GET',
                    url: '/api/event/geteventdetails/:id'
                },
                getUserEventDetails: {
                    method: 'GET',
                    url: '/api/event/getusereventdetails/:id'
                },
                getAdminEventDetails: {
                    method: 'GET',
                    url: '/api/event/getadmineventdetails/:id'
                },
                getVolunteerInfo: {
                    method: 'GET',
                    url: '/api/event/getvolunteerinfo/:eventId',
                    isArray: true
                }
            });
            this.eventCommentResources = $resource("/api/eventComment/:id", null, {
                signUpToVolunteer: {
                    method: "PUT",
                    url: "/api/eventComment/:id",
                    isArray: false
                }
            });
            this.eventLocationResources = $resource("/api/eventLocation/:id", null, {
                saveUserEventEdit: {
                    method: 'POST',
                    url: '/api/eventLocation/saveusereventedit/:id'
                }, 
                saveAddedEventLocation: {
                    method: 'POST',
                    url: '/api/eventLocation/saveaddedeventlocation/:id'  //eventId
                },
                saveLocation: {
                    method: 'POST',
                    url: '/api/eventLocation/savelocation/:id'  //locationId
                }
            });
        }

        signUpToVolunteer(eventId, hasAdminApproved) {
            return this.eventCommentResources.signUpToVolunteer({ id: eventId }, hasAdminApproved).$promise;
        }

        getVolunteerInfo(eventId) {
            debugger;
            return this.eventsResources.getVolunteerInfo({ eventId }).$promise;
        }

        getLocation(locationId) {
            return this.eventLocationResources.get({ id: locationId }).$promise;
        }

        //edits an existing event, NOT including the location
        saveUserEventEdit(eventId, event) {
            return this.eventLocationResources.saveUserEventEdit({ id: eventId }, event).$promise;
        }

        //save an event location
        saveAddedEventLocation(eventId, locationToSave) {
            return this.eventLocationResources.saveAddedEventLocation({ id: eventId }, locationToSave).$promise;
        }

        saveLocation(locationIdFrom, locationToSave) {
            debugger;
            return this.eventLocationResources.saveLocation({ id: locationIdFrom }, locationToSave).$promise;
        }


        getActiveEvents() {
            return this.eventsResources.getActiveEvents().$promise;
        }

        getHistoryEvents() {
            return this.eventsResources.getHistoryEvents().$promise;
        }

        getUserEvents() {

            return this.eventsResources.getUserEvents().$promise;
        }

        getUserHistoryEvents() {
            return this.eventsResources.getUserHistoryEvents().$promise;
        }

        //this is for admin
        getAllEvents() {

            return this.eventsResources.getAllEvents().$promise;
        }

        getAllHistoryEvents() {
            return this.eventsResources.getAllHistoryEvents().$promise;
        }

        //this does not do any filter at all
        getEventDetails(eventId) {
            return this.eventsResources.getEventDetails({ id: eventId }).$promise;
        }

        getUserEventDetails(eventId) {
            return this.eventsResources.getUserEventDetails({ id: eventId }).$promise;
        }

        getAdminEventDetails(eventId) {
            return this.eventsResources.getAdminEventDetails({ id: eventId }).$promise;
        }

        searchByCity(city) {
            return this.eventsResources.search({ city: city }).$promise;
        }

        searchActiveHistoryEventsByCity(city) {
            return this.eventsResources.searchActiveHistoryEvents({ city: city }).$promise;
        }

        searchMyActiveEvents(city) {
            return this.eventsResources.searchMyActiveEvents({ city: city }).$promise;
        }

        searchMyActiveHistoryEvents(city) {
            return this.eventsResources.searchMyActiveHistoryEvents({ city: city }).$promise;
        }

        searchAllEvents(city) {
            return this.eventsResources.searchAllEvents({ city: city }).$promise;
        }

        searchAllHistoryEvents(city) {
            return this.eventsResources.searchAllHistoryEvents({ city: city }).$promise;
        }

        //save an event's comment
        saveEventComment(eventId, commentToSave) {
            return this.eventCommentResources.save({ id: eventId }, commentToSave).$promise;
        }

        voteEvent(eventId, voteValue) {
            return this.eventsResources.vote({ id: eventId }, voteValue).$promise;
        }

        //save a new event and its single location
        saveEvent(event) {
            return this.eventsResources.save(event).$promise;
        }

       
        deleteEvent(eventId) {
            return this.eventsResources.remove({ id: eventId }).$promise;
        }
    }

    angular.module("SupportApp").service("eventServices", EventServices);
}