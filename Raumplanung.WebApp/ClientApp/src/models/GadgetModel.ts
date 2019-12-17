import { Model } from '@vuex-orm/core'

export default class Gadget extends Model {
  // This is the name used as module name of the Vuex Store.
  public static entity = 'gadgets'
  public static primaryKey = 'Id'

  // List of all fields (schema) of the post model. `this.attr` is used
  // for the generic field type. The argument is the default value.
  public static fields () {
    return {
      Id: this.attr(null),
      Title: this.attr(''),
      SuppliedBy: this.attr(null)
    }
  }
}

export interface GadgetModel {
  Id: number
  Title: string
  SuppliedBy: string
}
