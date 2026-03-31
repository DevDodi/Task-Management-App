/**
 * @typedef {Object} User
 * @property {string} id
 * @property {string} email
 * @property {string} password
 */

/**
 * @typedef {Object} Project
 * @property {string} id
 * @property {string} name
 * @property {string} ownerId
 */

/**
 * @typedef {Object} Task
 * @property {string} id
 * @property {string} title
 * @property {string} description
 * @property {TaskStatus} status
 * @property {string} assignedUser
 * @property {string} assignedProject
 */

/** @typedef {typeof TaskStatus[keyof typeof TaskStatus]} TaskStatus */
    export const TaskStatus = ({
    NotCompleted: 0,
    InProgress: 1,
    Completed: 2,
    });


/**
 * @typedef {Object} TaskLog
 * @property {string} id
 * @property {LogAction} action
 * @property {string} taskId
 * @property {string} changedByUser
 */

/** @typedef {typeof LogAction[keyof typeof LogAction]} LogAction */
    export const LogAction = ({
    Updated: 0,
    Created: 1,
    StatusChange: 2,
    Assigned: 3,
    });