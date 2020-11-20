/**
 * Caff Store API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { FileDto } from './fileDto';
import { UserDto } from './userDto';


export interface CaffItemDto { 
    id?: number;
    title: string;
    description: string;
    downloadedTimes?: number;
    previewFile?: FileDto;
    createdBy?: UserDto;
}

