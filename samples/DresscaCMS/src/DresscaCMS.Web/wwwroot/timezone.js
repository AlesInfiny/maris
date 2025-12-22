window.timezoneHelper = {
    getTimezoneOffset: function () {
        const offsetMinutes = new Date().getTimezoneOffset();
        return -offsetMinutes;
    }
};
